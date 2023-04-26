using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private Transform _fireballLaunchTransform;

    [HideInInspector] public bool IsActiveCharacter;
    [HideInInspector] public Vector2 Move { get; set; }
 
    private Vector3 _moveDir = Vector3.zero;
    private Vector3 _targetDirection;
    private Quaternion _freeRotation;
    private CharacterController _cc;
 
    private void Start()
    {
        _cc = GetComponent<CharacterController>();
        // Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (IsActiveCharacter)
        {
            float movementDirectionY = _moveDir.y;
            bool isRunning = false;
            isRunning = Input.GetKey(KeyCode.LeftShift);
    
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
            _moveDir.y = movementDirectionY;
        }
        if (!_cc.isGrounded)
            _moveDir.y -= _playerInfo.Gravity * Time.deltaTime;

        if (Move != Vector2.zero)
            _cc.Move(_moveDir * Time.deltaTime);
    }

    public void LaunchFireball()
    {
        var camForward = Camera.main.transform.TransformDirection(Vector3.forward);
        Quaternion dir = Quaternion.LookRotation(camForward);
        var go = Instantiate(_playerInfo.YangProjectile, _fireballLaunchTransform.position, dir);
        Destroy(go, 10f);
    }
}