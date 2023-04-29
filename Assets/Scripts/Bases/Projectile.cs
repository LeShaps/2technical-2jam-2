using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 150f;
    private Rigidbody _rb;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.velocity = _speed * transform.forward;
    }

    public float Speed
    {
        get {
            return _speed;
        }
        set {
            _speed = value;
        }
    }
}
