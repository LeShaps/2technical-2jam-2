using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UltimateLoad : MonoBehaviour
{
    [SerializeField] int _yinChargeAmount = 5;
    [SerializeField] int _yangChargeAmount = 5;
    [SerializeField] int _loseChargeAmount = 5;

    [SerializeField] int _yangCriticalChargeAmount = 20;
    [SerializeField] Slider _yinSlider;
    [SerializeField] Slider _yangSlider;
    [SerializeField] RawImage _yinUltimateImage;
    [SerializeField] RawImage _yangUltimateImage;
    [SerializeField] GameObject _yinChargeImage;
    [SerializeField] GameObject _yangChargeImage;

    float _yinFill = 0f;
    float _yangFill = 0f;
    public static UltimateLoad Instance { get; private set; }

    void Awake() => Instance = this;

    void CheckBothCharged()
    {
        if (_yinFill >= 100 &&  _yangFill >= 100)
        {
            _yinUltimateImage.color = new Color {
                r = _yinUltimateImage.color.r,
                g = _yinUltimateImage.color.g,
                b = _yinUltimateImage.color.b,
                a = 0
            };
            _yangUltimateImage.color = new Color {
                r = _yangUltimateImage.color.r,
                g = _yangUltimateImage.color.g,
                b = _yangUltimateImage.color.b,
                a = 0
            };
            GameManager.Instance.ChangeState(GameState.Win);
        }
    }

    public void LooseCharge(bool yin)
    {
        if (GameManager.Instance.State == GameState.Win || GameManager.Instance.State == GameState.End)
            return;
        
        if (yin && GameManager.Instance.ActivePlayer == Player.Yin && _yinFill < 100)
        {
            _yinFill -= _loseChargeAmount;
            if (_yinFill < 0) _yinFill = 0;
            _yinSlider.value = _yinFill;
        }
        else if (!yin && GameManager.Instance.ActivePlayer == Player.Yang && _yangFill < 100)
        {
            _yangFill -= _loseChargeAmount;
            if (_yangFill < 0) _yangFill = 0;
            _yangSlider.value = _yangFill;
        }
    }

    public void AddYinCharge()
    {
        if (_yinFill < 100)
        {
            _yinFill += _yinChargeAmount;
            _yinSlider.value = _yinFill;
        }
        if (_yinFill >= 100) {
            _yinUltimateImage.color = new Color {
                r = _yinUltimateImage.color.r,
                g = _yinUltimateImage.color.g,
                b = _yinUltimateImage.color.b,
                a = 100
            };
            _yinChargeImage.SetActive(true);
            CheckBothCharged();
        }
    }

    public void AddYangCharge(bool isCritical = false)
    {
        if (_yangFill < 100) {
            _yangFill += isCritical ? _yangCriticalChargeAmount : _yangChargeAmount;
            _yangSlider.value = _yangFill;
        }
        if (_yangFill >= 100)
        {
            _yangUltimateImage.color = new Color {
                r = _yangUltimateImage.color.r,
                g = _yangUltimateImage.color.g,
                b = _yangUltimateImage.color.b,
                a = 100
            };
            _yangChargeImage.SetActive(true);
            CheckBothCharged();
        }
    }
}
