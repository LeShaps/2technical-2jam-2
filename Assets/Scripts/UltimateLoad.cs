using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UltimateLoad : MonoBehaviour
{
    private float _yinFill = 0f;
    private float _yangFill = 0f;

    [SerializeField] private TextMeshProUGUI _textInfo;
    [SerializeField] private Slider _yinSlider;
    [SerializeField] private Slider _yangSlider;
    [SerializeField] private RawImage _ultimateImage;

    public void ActivateUltimate()
    {
        if (_yinFill != 100 || _yangFill != 100)
            return;
        else {
            EndZone[] WinZones = FindObjectsOfType<EndZone>();
            if (WinZones.All(x => x.Activated)) {
                // Put win here
            } else {
                _textInfo.alpha = 1f;
                _textInfo.text = "One of the character is not on the ultimate point";
                StartCoroutine(FadeText());
            }
        }
    }

    private void CheckFullCharge() {
        if (_yinFill >= 100 &&  _yangFill >= 100) {
            _textInfo.alpha = 1f;
            _textInfo.text = "The ultimate is ready!";
            StartCoroutine(FadeText());
            
            _ultimateImage.color = new Color {
                r = _ultimateImage.color.r,
                g = _ultimateImage.color.g,
                b = _ultimateImage.color.b,
                a = 100
            };
        }
    }

    public void AddCharge(float amount, bool yin) {
        if (yin && _yinFill < 100) {
            _yinFill += amount;
            _yinSlider.value = _yinFill;

            if (_yinFill >= 100) {
                _textInfo.alpha = 1f;
                _textInfo.text = "Yin is charged";
                StartCoroutine(FadeText());
                CheckFullCharge();
            }
        }

        if (!yin && _yangFill < 100) {
            _yangFill += amount;
            _yangSlider.value = _yangFill;

            if (_yangFill >= 100) {
                _textInfo.alpha = 1f;
                _textInfo.text = "Yang is charged";
                StartCoroutine(FadeText());
                CheckFullCharge();
            }
        }
    }

    public IEnumerator FadeText() {
        while (_textInfo.alpha > 0) {
            _textInfo.alpha -= (Time.deltaTime / 2);
            yield return null;
        }
    }
}
