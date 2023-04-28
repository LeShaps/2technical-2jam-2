using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] float _weakPointMaxDistance = 1f;
    [SerializeField] Transform _weakPointTransform;
    private bool _offensiveStance = true;
    private bool _isAlive = true;

    public static Boss Instance { get; private set; }
    void Awake() => Instance = this;

    private void Start()
    {
        StartCoroutine(LoopAllPatterns());
    }

    private void Update()
    {
        if (_offensiveStance) {
            // Make offensive patterns, make attackable
        } else {
            // Make defensive patterns, make invincible
        }
    }

    public bool HasHitCloseToWeakPoint(Vector3 hitPos)
    {
        Debug.Log((_weakPointTransform.position - hitPos).magnitude);
        return (_weakPointTransform.position - hitPos).magnitude < _weakPointMaxDistance;
    }

    private IEnumerator LoopAllPatterns()
    {
        while (_isAlive) {
            _offensiveStance = !_offensiveStance;
            yield return new WaitForSeconds(20);
        }
    }

    private void PatternFocus()
    {
        
    }

    private void PatternCircle()
    {
        
    }
}
