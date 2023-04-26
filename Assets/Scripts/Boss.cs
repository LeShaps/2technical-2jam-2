using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    UltimateLoad Ul;
    bool OffensiveStance = true;
    bool isAlive = true;

    private void Start() {
        Ul = FindObjectOfType<UltimateLoad>();
        StartCoroutine(StanceChange());
    }

    public void InflictDamage(bool isCritical) {
        if (isCritical) {
            Ul.AddCharge(5, false);
        } else {
            Ul.AddCharge(1, false);
        }
    }

    private void Update() {
        if (OffensiveStance) {
            // Make offensive patterns, make attackable
        } else {
            // Make defensive patterns, make invincible
        }
    }

    public IEnumerator StanceChange() {
        while (isAlive) {
            OffensiveStance = !OffensiveStance;
            yield return new WaitForSeconds(20);
        }
    }
}
