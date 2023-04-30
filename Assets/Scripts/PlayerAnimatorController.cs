using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    Animator _animator;
    bool _isRunning = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        EventManager.AddListener("Attack", OnAttack);
        EventManager.AddListener("Running", OnRunning);
        EventManager.AddListener("StopRunning", OnStopRunning);
    }

    private void OnAttack()
    {
        _animator.SetTrigger("Attack");
    }

    private void OnRunning()
    {
        if (_isRunning)
            return;
            
        _isRunning = true;
        if (GameManager.Instance.ActivePlayer == Player.Yin)
            _animator.SetBool("YinIsRunning", true);
        else
            _animator.SetBool("YangIsRunning", true);
    }

    private void OnStopRunning()
    {
        if (!_isRunning)
            return;

        _isRunning = false;

        if (GameManager.Instance.ActivePlayer == Player.Yin)
            _animator.SetBool("YinIsRunning", true);
        else
            _animator.SetBool("YangIsRunning", true);
    }
}