using UnityEngine;
using UnityEngine.InputSystem;

public class HerosController : MonoBehaviour
{
    [SerializeField] private WaterShield _yinWaterShield;
    [SerializeField] private GameObject _yangVcam;

    private float _refillCharge;
    private bool _isRefilling = false;

    private void Update() {
        if (_isRefilling) {
            _refillCharge += Time.deltaTime;
            if (_refillCharge > 8f) {
                _isRefilling = false;
                _refillCharge = 0;
            }
        }
    }

    public void OnSwitchCharacter(InputValue value)
    {
        GameManager.Instance.SwitchActivePlayer();
        bool activeYang = GameManager.Instance.ActivePlayer == Player.Yang;

        _yangVcam.SetActive(activeYang);

        if (activeYang && _yinWaterShield.isActiveAndEnabled)
        {
            _yinWaterShield.Desactivate();
        }
    }

    public void OnMove(InputValue value)
    {
        if (GameManager.Instance.ActivePlayer == Player.Yang)
        {
            GameManager.Instance.ActivePlayerController.Move = value.Get<Vector2>();
        }
        else
        {
            GameManager.Instance.ActivePlayerController.Move = value.Get<Vector2>();
        }
    }

    public void OnFire(InputValue value)
    {
        if (Time.timeSinceLevelLoad == 0)
            return;

        if (GameManager.Instance.ActivePlayer == Player.Yin)
        {
            if (!_isRefilling) {
                _yinWaterShield.ActivateFewSeconds();
                _isRefilling = true;
            }
        }
        else
        {
            GameManager.Instance.ActivePlayerController.LaunchFireball();
        }
    }
}
