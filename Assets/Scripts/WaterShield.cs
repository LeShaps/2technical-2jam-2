using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShield : MonoBehaviour
{
    UltimateLoad Ul;

    [SerializeField]
    private float ShieldLife = 100f;

    private void Start() {
        Ul = FindObjectOfType<UltimateLoad>();
    }

    private void Update() {
        if (ShieldLife < 100) {
            ShieldLife += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        BossProjectile projectile;

        if (collision.gameObject.Contains(out projectile)) {
            Destroy(projectile.gameObject);
            Ul.AddCharge(1, true);
            ShieldLife -= 2;
        }
    }
}
