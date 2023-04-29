using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject _orbPrefab;
    [SerializeField] float _singleOrbPace = 1f;
    [SerializeField] float _singleOrbSpeed = 200f;
    [SerializeField] float _singleOrbDuration = 2f; // try to optimize by set the minimim duration needed

    [SerializeField] Transform _weakPointTransform;
    [SerializeField] float _weakPointMaxDistance = 1f;
    [SerializeField] SplineContainer _splineContainer;
    private Spline _splines;
    private float _fireOrbTimer;
    // private bool _offensiveStance = true;

    public static Boss Instance { get; private set; }
    void Awake() => Instance = this;

    public void StartPatterns()
    {
        StartCoroutine(LoopAllPatterns());
    }

    const float freq = 3f;
    private bool _faceForward = true;
    float currentTime = 0f;
    float timeToMove = 2f;

    private void Update()
    {
        // if (_offensiveStance) {
        // } else {
        // }

        // float theta = Time.timeSinceLevelLoad / freq;
        // float t = Mathf.Sin(theta) * 0.5f + 0.5f;
        // if (t < .1f || t >.9f) _faceForward = !_faceForward;
        // _splineContainer.Evaluate(t, out var position, out var tangent, out var normal);
        // transform.position = new Vector3(position.x, transform.position.y, position.z);
        // Quaternion splineNextDir = _faceForward ? Quaternion.LookRotation(tangent) : Quaternion.LookRotation(-tangent);
        // transform.rotation = splineNextDir;

        if (currentTime <= timeToMove)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Lerp(0, 1f, currentTime / timeToMove);
            t = _faceForward ? t : 1-t;
            _splineContainer.Evaluate(t, out var position, out var tangent, out var normal);
            // transform.position = new Vector3(position.x, transform.position.y, position.z);
            Quaternion splineNextRot = _faceForward ? Quaternion.LookRotation(tangent) : Quaternion.LookRotation(-tangent);
            // transform.rotation = Quaternion.Lerp(transform.rotation, splineNextRot, t);
        }
        else
        {
            currentTime = 0;
            _faceForward = !_faceForward;
        }

        // Projectiles
        _fireOrbTimer -= Time.deltaTime;
        if (_fireOrbTimer < 0)
        {
            _fireOrbTimer = _singleOrbPace;
            Vector3 playerPos = GameManager.Instance.ActivePlayerController.transform.position;
            Vector3 orbDir = (playerPos - transform.position).normalized;
            Quaternion rota = Quaternion.LookRotation(orbDir);
            transform.rotation = rota;
            var go = Instantiate(_orbPrefab, transform.position + (transform.forward * 1.1f), rota);
            go.GetComponent<Projectile>().Speed = _singleOrbSpeed;
            Destroy(go, _singleOrbDuration);
        }
    }

    public bool HasHitCloseToWeakPoint(Vector3 hitPos)
    {
        Debug.Log((_weakPointTransform.position - hitPos).magnitude);
        return (_weakPointTransform.position - hitPos).magnitude < _weakPointMaxDistance;
    }

    private IEnumerator LoopAllPatterns()
    {
        // _offensiveStance = !_offensiveStance;
        // yield return new WaitForSeconds(20);
        StartCoroutine(PatternFocus());
        yield return null;
    }

    private IEnumerator PatternFocus()
    {
        
        yield return new WaitForSeconds(5);
    }

    private void PatternCircle()
    {
        
    }
}
