using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private Transform _fireballLaunchTransform;

    [HideInInspector] public Player Player { get; set; }
    [HideInInspector] public Vector2 Move { get; set; }
 
    private Vector3 _moveDir = Vector3.zero;
    private Vector3 _targetDirection;
    private Quaternion _freeRotation;
    private CharacterController _cc;
    
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        // Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetAnimatorBoolVariable(string name, bool boolValue)
    {
        if (_animator == null) return;
        _animator.SetBool(name, boolValue);
    }

    public void SetAnimatorTriggerVariable(string name)
    {
        if (_animator == null) return;
        _animator.SetTrigger(name);
    }

    private void FixedUpdate()
    {
        if (Player == GameManager.Instance.ActivePlayer)
        {
            float movementDirectionY = _moveDir.y;
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            float horizontal = Move.x;
            float vertical = Move.y;
            if (vertical < 0)
                vertical *= -1;
            if (horizontal < 0)
                horizontal *= -1;
            float move = vertical + horizontal;
    
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            _moveDir = forward * move;
            _moveDir.Normalize();
            _moveDir *= isRunning ? _playerInfo.RunningSpeed : _playerInfo.WalkingSpeed;
    
            // Update player rotation
            var camForward = Camera.main.transform.TransformDirection(Vector3.forward);
            camForward.y = 0;
            var camRight = Camera.main.transform.TransformDirection(Vector3.right);
            _targetDirection = Move.x * camRight + Move.y * camForward;

            if(Move != Vector2.zero && _targetDirection.magnitude > 0.1f)
            {
                Vector3 lookDirection = _targetDirection.normalized;
                _freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
                var diferenceRotation = _freeRotation.eulerAngles.y - transform.eulerAngles.y;
                var eulerY = transform.eulerAngles.y;
    
                if (diferenceRotation < 0 || diferenceRotation > 0)
                    eulerY = _freeRotation.eulerAngles.y;
                var euler = new Vector3(0, eulerY, 0);
    
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), _playerInfo.RotationPower * Time.deltaTime);
            }
            else
            {
                SetAnimatorBoolVariable("Running", false);
            }
            _moveDir.y = movementDirectionY;
        }
        if (!_cc.isGrounded)
            _moveDir.y -= _playerInfo.Gravity * Time.deltaTime;

        if (Move != Vector2.zero)
            _cc.Move(_moveDir * Time.deltaTime);
    }

    public void LaunchFireball()
    {
        EventManager.TriggerEvent("Attack");
        var camForward = Camera.main.transform.TransformDirection(Vector3.forward);

        // Rotate player before projectile launch
        transform.rotation = Quaternion.LookRotation( new Vector3(camForward.x, 0, camForward.z) );
        
        Quaternion projectileDir = Quaternion.LookRotation(camForward);
        var go = Instantiate(_playerInfo.YangProjectile, _fireballLaunchTransform.position, projectileDir);
        Destroy(go, 2f);
    }
}