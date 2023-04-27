using UnityEngine;
using System.Collections;

public class WaterShield : MonoBehaviour
{
    private UltimateLoad _ul;
    [SerializeField] GameObject _shieldHitVFXContainer;
    // [SerializeField] private float _shieldRegenerationSpeed = 1f;
    // private float _shieldLife = 100f;

    private void Start()
    {
        _ul = FindObjectOfType<UltimateLoad>();
    }

    // private void Update()
    // {
        // if (_shieldLife < 100)
        // {
        //     _shieldLife += _shieldRegenerationSpeed * Time.deltaTime;
        // }
    // }

    public void ToggleActive()
    {
        var doActive = !gameObject.activeInHierarchy;
        if (doActive)
        {
            gameObject.SetActive(true);
            StartCoroutine(ActiveShield());
        }
        else
        {
            Desactivate();
        }
    }

    public void Desactivate()
    {
        StartCoroutine(DisableShield());
    }

    const float _activationSpeed = 2f;
    const float _desactivationSpeed = 4f;
    private float _activationTime;

    private IEnumerator ActiveShield()
    {
        _activationTime = 0;
        while (_activationTime < 1f)
        {
            _activationTime += Time.deltaTime * _activationSpeed;
            var t = EaseOutBack(_activationTime);
            transform.localScale = new Vector3(t,t,t);
            yield return null;
        }
    }

    private IEnumerator DisableShield()
    {
        _activationTime = 1f;
        while (_activationTime > 0)
        {
            _activationTime -= Time.deltaTime * _desactivationSpeed;
            var t = EaseOutBack(_activationTime);
            transform.localScale = new Vector3(t,t,t);
            if (_activationTime <= 0)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Projectile projectile; // TODO: for testing purpose
        // BossProjectile projectile;
        if (collision.gameObject.Contains(out projectile))
        {
            ContactPoint contact = collision.contacts[0];
            Destroy(projectile.gameObject);
            _ul.AddCharge(1, true);
            // _shieldLife -= 2;
            foreach (ParticleSystem vfx in _shieldHitVFXContainer.GetComponentsInChildren<ParticleSystem>())
            {
                Instantiate(vfx, contact.point, Quaternion.identity);
                vfx.Play();
            }
        }
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;  
        float c3 = c1 + 1;  
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }
}
