using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    private void OnCollisionEnter(Collision collision) {
        Boss BossCharacter;

        if (collision.gameObject.Contains(out BossCharacter)) {
            // Determine here if it's critical or not
            BossCharacter.InflictDamage(true);
        }
    }
}
