using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    UltimateLoad Ul;

    private void Start() {
        Ul = FindObjectOfType<UltimateLoad>();
    }

    public void InflictDamage(bool isCritical) {
        if (isCritical) {
            Ul.AddCharge(5, false);
        } else {
            Ul.AddCharge(1, false);
        }
    }
}
