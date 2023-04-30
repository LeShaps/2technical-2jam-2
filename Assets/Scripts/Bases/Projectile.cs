using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float _speed = 150f;
    protected Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = _speed * transform.forward;  
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
