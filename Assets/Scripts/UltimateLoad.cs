using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UltimateLoad : MonoBehaviour {
    private float YinFill = 0f;
    private float YangFill = 0f;

    [SerializeField]
    private TextMeshProUGUI TextInfo;
    [SerializeField]
    private Slider YinSlider;
    [SerializeField]
    private Slider YangSlider;
    [SerializeField]
    private RawImage UltimateImage;

    public void ActivateUltimate() {
        if (YinFill != 100 || YangFill != 100) return;
        else {
            EndZone[] WinZones = FindObjectsOfType<EndZone>();
            if (WinZones.All(x => x.Activated)) {
                // Put win here
            } else {
                TextInfo.alpha = 1f;
                TextInfo.text = "One of the character is not on the ultimate point";
                StartCoroutine(FadeText());
            }
        }
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.S)) {
            AddCharge(10, true);
        }
        if (Input.GetKeyUp(KeyCode.U)) {
            AddCharge(10, false);
        }
    }

    private void CheckFullCharge() {
        if (YinFill >= 100 &&  YangFill >= 100) {
            TextInfo.alpha = 1f;
            TextInfo.text = "The ultimate is ready!";
            StartCoroutine(FadeText());
            
            UltimateImage.color = new Color {
                r = UltimateImage.color.r,
                g = UltimateImage.color.g,
                b = UltimateImage.color.b,
                a = 100
            };
        }
    }

    public void AddCharge(float amount, bool yin) {
        if (yin && YinFill < 100) {
            YinFill += amount;
            YinSlider.value = YinFill;

            if (YinFill >= 100) {
                TextInfo.alpha = 1f;
                TextInfo.text = "Yin is charged";
                StartCoroutine(FadeText());
                CheckFullCharge();
            }
        }

        if (!yin && YangFill < 100) {
            YangFill += amount;
            YangSlider.value = YangFill;

            if (YangFill >= 100) {
                TextInfo.alpha = 1f;
                TextInfo.text = "Yang is charged";
                StartCoroutine(FadeText());
                CheckFullCharge();
            }
        }
    }

    public IEnumerator FadeText() {
        while (TextInfo.alpha > 0) {
            TextInfo.alpha -= (Time.deltaTime / 2);
            yield return null;
        }
    }
}
