using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] float _weakPointMaxDistance = 1f;
    [SerializeField] Transform _weakPointTransform;
    private UltimateLoad _ul;
    private bool _offensiveStance = true;
    private bool _isAlive = true;

    private void Start()
    {
        _ul = FindObjectOfType<UltimateLoad>();
        StartCoroutine(StanceChange());
    }

    public void InflictDamage(bool isCritical = false)
    {
        _ul.AddYangCharge(isCritical);
    }

    private void Update()
    {
        if (_offensiveStance) {
            // Make offensive patterns, make attackable
        } else {
            // Make defensive patterns, make invincible
        }
    }

    public IEnumerator StanceChange()
    {
        while (_isAlive) {
            _offensiveStance = !_offensiveStance;
            yield return new WaitForSeconds(20);
        }
    }

    public bool HasHitCloseToWeakPoint(Vector3 hitPos)
    {
        Debug.Log((_weakPointTransform.position - hitPos).magnitude);
        return (_weakPointTransform.position - hitPos).magnitude < _weakPointMaxDistance;
    }
}
