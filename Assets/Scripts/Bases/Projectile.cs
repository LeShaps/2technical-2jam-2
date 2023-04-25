using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody body;

    [SerializeField]
    public float Speed = 10f;

    private void Start() {
        body = GetComponent<Rigidbody>();        
    }

    private void Update() {
        body.velocity += Speed * Time.deltaTime * Vector3.forward;
    }
}
