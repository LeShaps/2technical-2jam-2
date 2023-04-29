using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float _speed = 150f;
    Rigidbody _rb;

    void Start()
    {
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
