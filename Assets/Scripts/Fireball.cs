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
        if (collision.gameObject.CompareTag("Yang"))
            return;
            
        bool playSmallAnim = true;
        Boss bossCharacter;
        ContactPoint contact = collision.contacts[0];

        if (collision.gameObject.Contains(out bossCharacter))
        {
            if (bossCharacter.HasHitCloseToWeakPoint(contact.point))
            {
                EventManager.TriggerEvent("TriggerSymbol", "Fire");
                Instantiate(_hitLargeVFX, contact.point, Quaternion.identity);
                SoundManager.GetInstance().PlaySoundGeneral("FireballHeavyHit");
                _hitLargeVFX.Play();
                UltimateLoad.Instance.AddYangCharge(true);
                playSmallAnim = false;
            }
            else
            {
                SoundManager.GetInstance().PlaySoundGeneral("FireballSmallHit");
                UltimateLoad.Instance.AddYangCharge();
            }
        }
        if (playSmallAnim)
        {
            Instantiate(_hitSmallVFX, contact.point, Quaternion.identity);
            _hitSmallVFX.Play();
        }
        Destroy(gameObject);
    }
}
