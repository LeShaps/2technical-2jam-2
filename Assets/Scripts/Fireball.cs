using UnityEngine;

public class Fireball : Projectile
{
    [SerializeField] ParticleSystem _explodeSmallVFX;
    [SerializeField] ParticleSystem _explodeLargeVFX;

    private void OnCollisionEnter(Collision collision) 
    {
        Boss bossCharacter;

        if (collision.gameObject.Contains(out bossCharacter)) {
            ContactPoint contact = collision.contacts[0];
            if (bossCharacter.HasHitCloseToWeakPoint(contact.point))
            {
                var go = Instantiate(_explodeLargeVFX, contact.point, Quaternion.identity);
                Destroy(go, 2f);
                _explodeLargeVFX.Play();
                bossCharacter.InflictDamage(true);
            }
            else
            {
                var go = Instantiate(_explodeSmallVFX, contact.point, Quaternion.identity);
                Destroy(go, 2f);
                _explodeSmallVFX.Play();
                bossCharacter.InflictDamage();
            }
            Destroy(gameObject);
        }
    }
}
