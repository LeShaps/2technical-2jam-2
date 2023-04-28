using UnityEngine;

public class Fireball : Projectile
{
    [SerializeField] ParticleSystem _hitSmallVFX;
    [SerializeField] ParticleSystem _hitLargeVFX;

    private void OnCollisionEnter(Collision collision) 
    {
        Boss bossCharacter;
        if (collision.gameObject.Contains(out bossCharacter))
        {
            ContactPoint contact = collision.contacts[0];
            if (bossCharacter.HasHitCloseToWeakPoint(contact.point))
            {
                EventManager.TriggerEvent("TriggerSymbol", "Fire");
                Instantiate(_hitLargeVFX, contact.point, Quaternion.identity);
                _hitLargeVFX.Play();
                bossCharacter.InflictDamage(true);
            }
            else
            {
                Instantiate(_hitSmallVFX, contact.point, Quaternion.identity);
                _hitSmallVFX.Play();
                bossCharacter.InflictDamage();
            }
            Destroy(gameObject);
        }
    }
}
