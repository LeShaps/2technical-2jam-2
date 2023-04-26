using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private PlayerController _yinController;
    [SerializeField] private PlayerController _yangController;
    [SerializeField] private GameObject _yinVcam;

    void Start()
    {
        _yinController.IsActiveCharacter = true;
    }

    public void OnSwitchCharacter(InputValue value)
    {
        _yinVcam.SetActive(!_yinVcam.activeInHierarchy);
        _yinController.IsActiveCharacter = !_yinController.IsActiveCharacter;
        _yangController.IsActiveCharacter = !_yangController.IsActiveCharacter;
        _yinController.Move = Vector2.zero;
        _yangController.Move = Vector2.zero;
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
}
