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
        StartCoroutine(RechargeShield());
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.S)) {
            ShieldLife -= 10;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        BossProjectile projectile;

        if (collision.gameObject.Contains(out projectile)) {
            Destroy(projectile.gameObject);
            Ul.AddCharge(1, true);
        }
    }

    private IEnumerator RechargeShield() {
        while (true) {
            if (ShieldLife < 100) {
                ShieldLife += Time.deltaTime;
                yield return null;
            }
        }
    }
}
