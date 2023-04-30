using UnityEngine;

public class BossProjectile : Projectile
{
    [SerializeField] ParticleSystem _bossHitParticle;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint firstContact = collision.contacts[0];
        Instantiate(_bossHitParticle, firstContact.point, Quaternion.identity);
        _bossHitParticle.Play();
        Destroy(gameObject);
    }
}
