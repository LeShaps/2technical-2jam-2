using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 100f;
    private Rigidbody _rb;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.velocity = _speed * transform.forward;
    }
}
