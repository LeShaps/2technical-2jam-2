using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.Mathf;

public class Boss : MonoBehaviour
{
    [SerializeField] float _singleOrbPace = 1f;
    [SerializeField] float _singleOrbSpeed = 200f;
    [SerializeField] float _singleOrbDuration = 2f;
    float _singleOrbTimer;
    [SerializeField] float _circleOrbsPace = 3f;
    [SerializeField] float _circleOrbSpeed = 100f;
    [SerializeField] float _circleOrbDuration = 6f;
    [SerializeField] float _circleOrbLaunchRadius = .3f;
    [SerializeField] int _circleOrbsCount = 10;
    float _circleOrbsTimer;
    [SerializeField] float _orbSpawnHeight = 1f;
    [SerializeField] GameObject _orbPrefab;
    Transform _projectilesParentTransform;
    
    [SerializeField] float _splineTravelDuration = 2f;
    [SerializeField] float _moveSpeed = 3f;
    bool _goesUpSpline = true;
    float _splineTravelTime;

    [SerializeField] Transform _weakPointTransform;
    [SerializeField] float _weakPointMaxDistance = 1f;
    [SerializeField] SplineContainer _splineContainer;
    Spline _splines;

    [SerializeField] float _patternDuration = 5f;
    float timeSinceLastPattern;
    public Pattern CurrentPattern { get; private set; }
    
    public static Boss Instance { get; set; }
    void Awake() => Instance = this;

    public void StartPatterns()
    {
        CurrentPattern = Pattern.PingPongSingleOrb;
        _projectilesParentTransform = new GameObject( gameObject.name + " Projectiles").transform;
    }

    public void StopPatterns()
    {
        Debug.Log("StopPatterns()");
    }

    Pattern NextPattern()
    {
        if (CurrentPattern == Pattern.PingPongSingleOrb)
            return Pattern.CenterOrbWaves;
        else
            return Pattern.PingPongSingleOrb;
    }

    void Update()
    {
        // Patterns switch
        timeSinceLastPattern += Time.deltaTime;
        if (timeSinceLastPattern >= _patternDuration)
        {
            timeSinceLastPattern = 0;
            CurrentPattern = NextPattern();

            Debug.Log($"Switch to pattern {CurrentPattern}");
        }
        
        if (CurrentPattern == Pattern.PingPongSingleOrb)
        {
            // Movement ping pong
            if (_splineTravelTime <= _splineTravelDuration)
            {
                _splineTravelTime += Time.deltaTime;
                float t = Lerp(0, 1f, _splineTravelTime / _splineTravelDuration);
                t = _goesUpSpline ? t : 1-t;
                _splineContainer.Evaluate(t, out var position, out var tangent, out var normal);
                transform.position = new Vector3(position.x, transform.position.y, position.z);
                // Quaternion splineNextRot = _goesUpSpline ? Quaternion.LookRotation(tangent) : Quaternion.LookRotation(-tangent);
                // transform.rotation = Quaternion.Lerp(transform.rotation, splineNextRot, t);
                // RotateToActivePlayer();
            }
            else
            {
                _splineTravelTime = 0;
                _goesUpSpline = !_goesUpSpline;
            }

            // Single Orb
            _singleOrbTimer -= Time.deltaTime;
            if (_singleOrbTimer < 0)
            {
                _singleOrbTimer = _singleOrbPace;
                
                Vector3 playerPos = GameManager.Instance.ActivePlayerController.transform.position;
                Vector3 spawnPos = transform.position;
                spawnPos.y = _orbSpawnHeight;
                Vector3 spawnDir = (playerPos - spawnPos).normalized;

                // float orbSpawnDistance = 1.1f;
                // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(spawnDir), Time.deltaTime);
                // SpawnOrb(spawnPos + spawnDir * orbSpawnDistance, spawnDir, _singleOrbSpeed, _singleOrbDuration);
            }
        }

        if (CurrentPattern == Pattern.CenterOrbWaves)
        {
            // Movement & rotation
            var nextPos = _splineContainer.EvaluatePosition(0.5f);
            nextPos.y = transform.position.y;
            if (Vector3.Distance(transform.position, nextPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * _moveSpeed);
                return;
            }
            // RotateToActivePlayer();

            // Orb Waves
            _circleOrbsTimer -= Time.deltaTime;
            if (_circleOrbsTimer < 0)
            {
                _circleOrbsTimer = _circleOrbsPace;
                float step = 2f / _circleOrbsCount;
                float v = 0.5f * step - 1f; // cover the -1/-1 range instead of 0/2
                for (int i = 0, x = 0, z = 0; i < _circleOrbsCount; i++, x++)
                {
                    if (x == _circleOrbsCount)
                    {
                        x = 0;
                        z += 1;
                        v = (z + 0.5f) * step - 1f;
                    }
                    float u = (x + 0.5f) * step - 1f;
                    
                    var spawnPos = Sphere(u, v);
                    spawnPos = spawnPos * _circleOrbLaunchRadius + transform.position;
                    var spawnDir = (spawnPos - transform.position).normalized;
                    spawnPos.y = _orbSpawnHeight;
                    // TODO: create variation (spawn offset)
                    SpawnOrb(spawnPos, spawnDir, _circleOrbSpeed, _circleOrbDuration);
                }
                // TODO: orb goes then comes back to the wizard
            }
        }
        RotateToActivePlayer();
    }

    void RotateToActivePlayer()
    {
        Vector3 playerPos = GameManager.Instance.ActivePlayerController.transform.position;
        Vector3 bossPos = transform.position;
        bossPos.y = playerPos.y;
        Vector3 lookPlayerDir = (playerPos - bossPos).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPlayerDir), Time.deltaTime);
    }

    private void SpawnOrb(Vector3 spawnPos, Vector3 spawnDir, float startSpeed, float lifeDuration)
    {
        var go = Instantiate(_orbPrefab, spawnPos, Quaternion.LookRotation(spawnDir), _projectilesParentTransform);
        go.GetComponent<Projectile>().Speed = startSpeed;
        Destroy(go, lifeDuration);
    }

    Vector3 Sphere(float u, float v)
    {
		Vector3 p;
		p.x = Sin(PI * u);
		p.y = 0f;
		p.z = Cos(PI * u);
		return p;
	}

    public bool HasHitCloseToWeakPoint(Vector3 hitPos)
    {
        Debug.Log((_weakPointTransform.position - hitPos).magnitude);
        return (_weakPointTransform.position - hitPos).magnitude < _weakPointMaxDistance;
    }
}

[Serializable]
public enum Pattern
{
    PingPongSingleOrb = 0,
    CenterOrbWaves = 1,
}