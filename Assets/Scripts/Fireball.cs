using UnityEngine;

public class Fireball : Projectile
{
    [SerializeField] ParticleSystem _hitSmallVFX;
    [SerializeField] ParticleSystem _hitLargeVFX;

    void Awake()
    {
        SoundManager.GetInstance().PlaySoundLocalized("Fireball", GetComponent<AudioSource>());
    }

    void OnCollisionEnter(Collision collision) 
    {
        ContactPoint contact = collision.contacts[0];

        Boss bossCharacter;
        if (collision.gameObject.Contains(out bossCharacter))
        {
            SoundManager.GetInstance().PlaySoundGeneral("FireballSmallHit");
            UltimateLoad.Instance.AddYangCharge();
            Instantiate(_hitSmallVFX, contact.point, Quaternion.identity);
            _hitSmallVFX.Play();
        }
        if (collision.gameObject.CompareTag("WeakPoint"))
        {
            EventManager.TriggerEvent("TriggerSymbol", "Fire");
            Instantiate(_hitLargeVFX, contact.point, Quaternion.identity);
            SoundManager.GetInstance().PlaySoundGeneral("FireballHeavyHit");
            _hitLargeVFX.Play();
            UltimateLoad.Instance.AddYangCharge(true);
        }
        Destroy(gameObject);
    }
}
