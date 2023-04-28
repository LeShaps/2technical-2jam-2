using UnityEngine;
using UnityEngine.InputSystem;

public class HerosController : MonoBehaviour
{
    [SerializeField] private PlayerController _yinController;
    [SerializeField] private WaterShield _yinWaterShield;

    [SerializeField] private PlayerController _yangController;
    [SerializeField] private GameObject _yangVcam;

    void Start()
    {
        _yinController.IsActiveCharacter = true;
    }

    public void OnSwitchCharacter(InputValue value)
    {
        bool activateYin = !_yinController.IsActiveCharacter;

        _yangVcam.SetActive(!activateYin);
        _yinController.IsActiveCharacter = activateYin;
        _yangController.IsActiveCharacter = !activateYin;
        _yinController.Move = Vector2.zero;
        _yangController.Move = Vector2.zero;

        if (!activateYin && _yinWaterShield.isActiveAndEnabled)
        {
            _yinWaterShield.Desactivate();
        }
    }

    public void OnMove(InputValue value)
    {
        if (_yinController.IsActiveCharacter)
        {
            _yinController.Move = value.Get<Vector2>();
        }
        else
        {
            _yangController.Move = value.Get<Vector2>();
        }
    }

    public void OnFire(InputValue value)
    {
        if (_yinController.IsActiveCharacter)
        {
            _yinWaterShield.ActivateFewSeconds();
        }
        else
        {
            _yangController.LaunchFireball();
        }
    }
}
