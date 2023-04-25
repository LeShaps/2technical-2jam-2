using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class UltimateLoad : MonoBehaviour {
    private float YinFill = 0f;
    private float YangFill = 0f;

    [SerializeField]
    private TextMeshProUGUI TextInfo;

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

    public void AddCharge(float amount, bool yin) {
        if (yin && YinFill < 100) {
            YinFill += amount;
            if (YinFill >= 100) {
                TextInfo.alpha = 1f;
                TextInfo.text = "Yin is charged";
                StartCoroutine(FadeText());
            }
        }
        if (!yin && YangFill < 100) {
            YangFill += amount;
            if (YangFill >= 100) {
                TextInfo.alpha = 1f;
                TextInfo.text = "Yang is charged";
                StartCoroutine(FadeText());
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
