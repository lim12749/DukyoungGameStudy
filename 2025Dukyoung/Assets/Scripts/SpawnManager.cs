using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs & Target")]
    public GameObject enemyPrefab;
    public Transform enemyTarget; // Tower 등 목적지

    [Header("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>(); // 정확 위치에 스폰

    [Header("Rules")]
    public bool autoStart = true;
    public float interval = 1.5f;   // 초
    public int perBurst = 1;        // 한번에 몇 마리
    public int maxAlive = 30;       // 동시 최대

    [Header("NavMesh")]
    public float navSampleRadius = 2f; // 0이면 샘플링 안함
    public bool requireNavMeshHit = true;

    [Header("Debug")]
    public bool logWarnings = true;

    int _aliveCount;
    Coroutine _loop;

    void Awake()
    {
        // 스폰포인트 자동 수집(비어있으면: 자식 + Tag=SpawnPoint)
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            foreach (Transform child in transform) spawnPoints.Add(child);
            foreach (var go in GameObject.FindGameObjectsWithTag("SpawnPoint"))
                if (!spawnPoints.Contains(go.transform)) spawnPoints.Add(go.transform);

            if (logWarnings && spawnPoints.Count == 0)
                Debug.LogWarning("[SpawnManager] SpawnPoint가 비어있습니다.");
        }

        if (!enemyTarget)
        {
            var tObj = GameObject.FindGameObjectWithTag("Tower");
            if (tObj) enemyTarget = tObj.transform;
        }
    }

    void OnEnable() { if (autoStart) StartSpawn(); }
    void OnDisable() { StopSpawn(); }

    public void StartSpawn()
    {
        if (_loop == null) _loop = StartCoroutine(SpawnLoop());
    }
    public void StopSpawn()
    {
        if (_loop != null) StopCoroutine(_loop);
        _loop = null;
    }

    IEnumerator SpawnLoop()
    {
        var wait = new WaitForSeconds(interval);
        while (true)
        {
            if (_aliveCount < maxAlive)
            {
                for (int i = 0; i < perBurst && _aliveCount < maxAlive; i++)
                    TrySpawnOne();
            }
            yield return wait;
        }
    }

    public bool TrySpawnOne()
    {
        if (!enemyPrefab || spawnPoints == null || spawnPoints.Count == 0) return false;

        // 1) 스폰 포인트 선택 (정확 위치)
        var sp = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector3 pos = sp.position;

        // 2) NavMesh 보정(선택)
        if (navSampleRadius > 0f)
        {
            if (NavMesh.SamplePosition(pos, out var hit, navSampleRadius, NavMesh.AllAreas))
                pos = hit.position;
            else if (requireNavMeshHit)
            {
                if (logWarnings) Debug.LogWarning("[SpawnManager] NavMesh.SamplePosition 실패 → 스폰 취소");
                return false;
            }
        }

        // 3) 소환
        var go = Instantiate(enemyPrefab, pos, Quaternion.identity);
        _aliveCount++;

        // 4) 목적지 설정(Enemy.cs 사용)
        var enemy = go.GetComponent<Enemy>();
        if (enemy && enemyTarget) enemy.target = enemyTarget;

        // 5) 파괴 시 살아있는 수 감소
        go.AddComponent<DespawnHook>().Init(this);
        return true;
    }

    class DespawnHook : MonoBehaviour
    {
        SpawnManager owner;
        public void Init(SpawnManager o) { owner = o; }
        void OnDestroy()
        {
            if (owner != null) owner._aliveCount = Mathf.Max(0, owner._aliveCount - 1);
        }
    }
}
