using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float _timeBetweenFire = 1f;
    private float _timer;

    void Start()
    {
        _timer = _timeBetweenFire;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _timer = _timeBetweenFire;
            var go = Instantiate(projectilePrefab, transform.position + (transform.forward * 1.1f), Quaternion.LookRotation(transform.forward));
            Destroy(go, 2f);
        }
    }
}
