using UnityEngine;

public class Fireball : Projectile
{
    [SerializeField] ParticleSystem _hitSmallVFX;
    [SerializeField] ParticleSystem _hitLargeVFX;

    private void OnCollisionEnter(Collision collision) 
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
                _hitLargeVFX.Play();
                bossCharacter.InflictDamage(true);
                playSmallAnim = false;
            }
            else
            {
                bossCharacter.InflictDamage();
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
