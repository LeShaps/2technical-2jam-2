using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YangCharacter : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        BossProjectile boss;

        if (collision.gameObject.Contains(out boss))
        {
            UltimateLoad.Instance.LooseCharge(false);
            Destroy(boss.gameObject);
        }
    }
}
