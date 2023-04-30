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

        SoundManager.GetInstance().Blend("Music1", "Music2", 3f);
    }

    public void OnMove(InputValue value)
    {
        PlayerController pc = GameManager.Instance.ActivePlayerController;
        pc.Move = value.Get<Vector2>();
        pc.SetAnimatorBoolVariable("Running", true);
    }

    public void OnFire(InputValue value)
    {
        PlayerController pc = GameManager.Instance.ActivePlayerController;

        if (Time.timeSinceLevelLoad < 0.1f)
            return;

        if (GameManager.Instance.ActivePlayer == Player.Yin)
        {
            if (!_isRefilling) {
                _yinWaterShield.ActivateFewSeconds();
                _isRefilling = true;
                pc.SetAnimatorTriggerVariable("WaterShield");
            }
        }
        else
        {
            pc.LaunchFireball();
            pc.SetAnimatorTriggerVariable("SingleAttack");
        }
    }
}
