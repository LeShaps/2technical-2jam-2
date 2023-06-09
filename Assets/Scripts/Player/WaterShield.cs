using UnityEngine;
using System.Collections;

public class WaterShield : MonoBehaviour
{
    [SerializeField] GameObject _waterShieldHitParticleContainer;
    [SerializeField] ParticleSystem _bossHitParticle;
    [SerializeField] float _activationDuration = 4f;
    [SerializeField] float _scaleUpSpeed = 2f;
    [SerializeField] float _fadeInSpeed = 1.5f;
    [SerializeField] float _fadeOutSpeed = 1.9f;
    [SerializeField] float _waterShieldFadeMinTreshold = .1f;
    [SerializeField] float _waterShieldFadeMaxTreshold = .9f;
    private UltimateLoad _ul;
    private float _toggleScaleTimer;
    private float _toggleFadeTimer;

    private void Awake()
    {
        _ul = FindObjectOfType<UltimateLoad>();
        transform.localScale = Vector3.zero;
        Shader.SetGlobalFloat("_Water_Shield_Reveal_Amount", _waterShieldFadeMinTreshold);
    }

    public void Desactivate()
    {
        StartCoroutine(FadeOutThenDesactivateShield());
    }

    public void ActivateFewSeconds()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            StartCoroutine(ActivateShieldThenDesactivate());
        }
    }

    private IEnumerator ActivateShieldThenDesactivate()
    {
        GameManager.Instance.ActivePlayerController.SetAnimatorTriggerVariable("CastShield");
        StartCoroutine(ScaleUpShield());
        StartCoroutine(FadeInShield());
        EventManager.TriggerEvent("TriggerShield", "Deplete");
        yield return new WaitForSeconds(_activationDuration);
        Desactivate();
        EventManager.TriggerEvent("TriggerShield", "Fill");
    }

    private IEnumerator ScaleUpShield()
    {
        _toggleScaleTimer = 0;
        while (_toggleScaleTimer < 1f)
        {
            _toggleScaleTimer += Time.deltaTime * _scaleUpSpeed;
            var t = EaseOutBack(_toggleScaleTimer);
            transform.localScale = new Vector3(t,t,t);
            yield return null;
        }
    }

    private IEnumerator FadeInShield()
    {
        _toggleFadeTimer = _waterShieldFadeMinTreshold;
        while (_toggleFadeTimer < _waterShieldFadeMaxTreshold)
        {
            _toggleFadeTimer += Time.deltaTime * _fadeInSpeed;
            var t = Mathf.Lerp(_waterShieldFadeMinTreshold, _waterShieldFadeMaxTreshold, _toggleFadeTimer);
            Shader.SetGlobalFloat("_Water_Shield_Reveal_Amount", t);
            yield return null;
        }
    }

    private IEnumerator FadeOutThenDesactivateShield()
    {
        _toggleFadeTimer = _waterShieldFadeMaxTreshold;
        while (_toggleFadeTimer > _waterShieldFadeMinTreshold)
        {
            _toggleFadeTimer -= Time.deltaTime * _fadeOutSpeed;
            var t = Mathf.Lerp(_waterShieldFadeMinTreshold, _waterShieldFadeMaxTreshold, _toggleFadeTimer);
            Shader.SetGlobalFloat("_Water_Shield_Reveal_Amount", t);
            if (_toggleFadeTimer <= _waterShieldFadeMinTreshold)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BossProjectile bossProjectile;
        if (collision.gameObject.Contains(out bossProjectile))
        {
            ContactPoint firstContact = collision.contacts[0];
            _ul.AddYinCharge();
            foreach (ParticleSystem vfx in _waterShieldHitParticleContainer.GetComponentsInChildren<ParticleSystem>())
            {
                Instantiate(vfx, firstContact.point, Quaternion.identity);
                vfx.Play();
            }
            SoundManager.GetInstance().PlayLocalizedPitch("Shield", GetComponent<AudioSource>());
            EventManager.TriggerEvent("TriggerSymbol", "Water");
        }
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;  
        float c3 = c1 + 1;  
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }
}
