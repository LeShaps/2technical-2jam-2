using UnityEngine;

public class WaterShield : MonoBehaviour
{
    private UltimateLoad _ul;
    [SerializeField] private float _shieldLife = 100f;

    private void Start()
    {
        _ul = FindObjectOfType<UltimateLoad>();
    }

    private void Update()
    {
        if (_shieldLife < 100) {
            _shieldLife += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BossProjectile projectile;
        if (collision.gameObject.Contains(out projectile))
        {
            Destroy(projectile.gameObject);
            _ul.AddCharge(1, true);
            _shieldLife -= 2;
        }
    }
}
