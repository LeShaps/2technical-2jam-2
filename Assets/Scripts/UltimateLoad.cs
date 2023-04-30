using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UltimateLoad : MonoBehaviour
{
    [SerializeField] int _yinChargeAmount = 5;
    [SerializeField] int _yangChargeAmount = 5;
    [SerializeField] int _loseChargeAmount = 5;

    [SerializeField] int _yangCriticalChargeAmount = 20;
    [SerializeField] private TextMeshProUGUI _textInfo;
    [SerializeField] private Slider _yinSlider;
    [SerializeField] private Slider _yangSlider;
    [SerializeField] private RawImage _ultimateImage;
    [SerializeField] private GameObject _yinChargeImage;
    [SerializeField] private GameObject _yangChargeImage;

    private float _yinFill = 0f;
    private float _yangFill = 0f;
    public static UltimateLoad Instance { get; private set; }

    void Awake() => Instance = this;

    // public void ActivateUltimate()
    // {
    //     if (_yinFill < 100 || _yangFill < 100)
    //         return;

    //     else {
    //         EndZone[] WinZones = FindObjectsOfType<EndZone>();
    //         if (WinZones.All(x => x.Activated))
    //         {
    //             GameManager.Instance.ChangeState(GameState.Win);
    //         } else {
    //             _textInfo.alpha = 1f;
    //             _textInfo.text = "One of the character is not on the ultimate point";
    //             StartCoroutine(FadeText());
    //         }
    //     }
    // }

    private void CheckFullCharge()
    {
        if (_yinFill >= 100 &&  _yangFill >= 100) {
            _textInfo.alpha = 1f;
            _textInfo.text = "The ultimate is ready!";
            // StartCoroutine(FadeText());

            GameManager.Instance.ChangeState(GameState.Win);
            
            _ultimateImage.color = new Color {
                r = _ultimateImage.color.r,
                g = _ultimateImage.color.g,
                b = _ultimateImage.color.b,
                a = 100
            };

            // GameManager.Instance.ChangeState(GameState.UltimateReady);
            GameManager.Instance.ChangeState(GameState.Win);
        }
    }

    public void LooseCharge(bool yin)
    {
        if (yin && GameManager.Instance.ActivePlayer == Player.Yin)
        {
            if (GameManager.Instance.ActivePlayer == Player.Yin)
            {                
                _yinFill -= _loseChargeAmount;
                if (_yinFill < 0) _yinFill = 0;
                _yinSlider.value = _yinFill;
            }
        }
        else
        {
            if (GameManager.Instance.ActivePlayer == Player.Yang)
            {
                _yangFill -= _loseChargeAmount;
                if (_yangFill < 0) _yangFill = 0;
                _yangSlider.value = _yangFill;
            }
        }
    }

    public void AddYinCharge()
    {
        if (_yinFill < 100)
        {
            _yinFill += _yinChargeAmount;
            _yinSlider.value = _yinFill;
            if (_yinFill >= 100) {
                _textInfo.alpha = 1f;
                _textInfo.text = "Yin is charged";
                _yinChargeImage.SetActive(true);
                StartCoroutine(FadeText());
                CheckFullCharge();
            }
        }
    }

    public void AddYangCharge(bool isCritical = false)
    {
        if (_yangFill < 100) {
            _yangFill += isCritical ? _yangCriticalChargeAmount : _yangChargeAmount;
            _yangSlider.value = _yangFill;
            if (_yangFill >= 100)
            {
                _textInfo.alpha = 1f;
                _textInfo.text = "Yang is charged";
                _yangChargeImage.SetActive(true);
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
