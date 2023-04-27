using UnityEngine;

public class HitEmitterScript : MonoBehaviour
{
    private float _timeLeft;

    public void Awake() {
        ParticleSystem system = GetComponent<ParticleSystem>();
        _timeLeft = system.main.startLifetimeMultiplier;
    }
     
    public void Update() {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0) {
            GameObject.Destroy(gameObject);
        }
    }
}