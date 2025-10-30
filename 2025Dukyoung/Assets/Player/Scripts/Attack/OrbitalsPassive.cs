using UnityEngine;
using System.Collections.Generic;

public class OrbitalsPassive : MonoBehaviour, IPassiveAttack
{
    [Header("Base")]
    public int   startCount   = 1;        // 시작 오비탈 개수
    public float radius       = 1.2f;     // 궤도 반경
    public float angularSpeed = 180f;     // °/s
    public float hitRadius    = 0.25f;    // 각 오비탈의 타격 반경
    public LayerMask enemyMask;

    [Header("Damage")]
    public float baseDamage  = 6f;
    public float dmgPerLevel = 2f;
    public float atkScale    = 0.15f;     // PlayerStats.attackDamage 계수

    [Header("Scaling per Level")]
    public float radiusPerLv = 0.10f;
    public float speedPerLv  = 10f;

    [Header("Visual (optional)")]
    public GameObject visualOrbPrefab;    // ← 여기에 ‘작은 구체’ 프리팹 넣기
    public float visualOrbScale = 0.3f;   // 구체 크기

    public string DisplayName => "Orbitals";

    Transform owner;
    PlayerStats stats;
    int level = 1;

    float angleDeg;
    int orbitalCount;

    readonly List<Vector3>    orbPos   = new();
    readonly List<Transform>  visuals  = new();

    // 중복 타격 방지
      readonly Dictionary<Collider, float> lastHit = new Dictionary<Collider, float>();
    public float perTargetHitInterval = 0.25f;
    float _gcTimer;

    // 내부용 임시 리스트
    static readonly List<Collider> _tmpKeys = new List<Collider>();

    public void Initialize(GameObject ownerGO, PlayerStats s)
    {
        owner = ownerGO.transform;
        stats = s;
        transform.SetParent(owner, false);
        orbitalCount = Mathf.Max(1, startCount);
        SyncVisuals();
    }

    public void Upgrade(int delta)
    {
        level = Mathf.Max(1, level + delta);

        // 레벨 2,4,6…에서 하나씩 추가되는 느낌
        if (level % 2 == 0) orbitalCount++;

        radius       = Mathf.Max(0.1f, radius + radiusPerLv * delta);
        angularSpeed = Mathf.Max(0f,   angularSpeed + speedPerLv * delta);

        SyncVisuals();
    }

    void Update()
    {
        if (!owner) return;

        angleDeg += angularSpeed * Time.deltaTime;

        EnsureListSize(orbitalCount, orbPos);
        float step = 360f / orbitalCount;

        for (int i = 0; i < orbitalCount; i++)
        {
            float a = angleDeg + step * i;
            Vector3 offset = new Vector3(Mathf.Cos(a * Mathf.Deg2Rad), 0f, Mathf.Sin(a * Mathf.Deg2Rad)) * radius;
            Vector3 pos = owner.position + offset;

            orbPos[i] = pos;
            TryHitEnemiesAt(pos);

            // 시각 오브젝트 동기화
            if (i < visuals.Count && visuals[i])
            {
                visuals[i].position = pos;
                visuals[i].rotation = Quaternion.LookRotation(offset.normalized, Vector3.up);
            }
        }
    }

void TryHitEnemiesAt(Vector3 pos)
    {
        var hits = Physics.OverlapSphere(pos, hitRadius, enemyMask, QueryTriggerInteraction.Collide);
        float now = Time.time;
        float dmg = CalcDamage();

        for (int i = 0; i < hits.Length; i++)
        {
            var col = hits[i];
            if (!col || !col.gameObject.activeInHierarchy) continue;

            // per-target 내부쿨다운
            if (lastHit.TryGetValue(col, out float last) && (now - last) < perTargetHitInterval)
                continue;

            // 데미지 처리
            var d = col.GetComponentInParent<IDamageable>();
            if (d != null)
            {
                d.TakeDamage(dmg);
                lastHit[col] = now;

                // 데미지 텍스트: 충돌지점 근처로 살짝 위
                Vector3 contact = col.ClosestPoint(pos);
                if ((contact - pos).sqrMagnitude < 0.0001f) contact = col.transform.position; // 안전장치
                DamageTextSpawner.Instance?.Spawn(contact + Vector3.up * 0.6f, dmg, false);
            }
        }

        // 가끔 죽은/파괴된 콜라이더 정리 (메모리/딕셔너리 누수 방지)
        _gcTimer += Time.deltaTime;
        if (_gcTimer >= 1.0f)
        {
            _gcTimer = 0f;
            _tmpKeys.Clear();
            foreach (var kv in lastHit)
                if (!kv.Key) _tmpKeys.Add(kv.Key);
            for (int k = 0; k < _tmpKeys.Count; k++)
                lastHit.Remove(_tmpKeys[k]);
        }
    }

    float CalcDamage()
    {
        float d = baseDamage + dmgPerLevel * (level - 1);
        if (stats) d += stats.attackDamage * atkScale;
        return d;
    }

    void SyncVisuals()
    {
        // 개수 맞추기
        while (visuals.Count < orbitalCount)
        {
            if (visualOrbPrefab)
            {
                var go = Instantiate(visualOrbPrefab, transform);
                go.transform.localScale = Vector3.one * visualOrbScale;
                visuals.Add(go.transform);
            }
            else
            {
                // 프리팹이 없더라도 리스트만 채워 시뮬 계속
                visuals.Add(null);
            }
        }
        while (visuals.Count > orbitalCount)
        {
            var last = visuals[visuals.Count - 1];
            if (last) Destroy(last.gameObject);
            visuals.RemoveAt(visuals.Count - 1);
        }

        // 위치 리스트도 맞추기
        EnsureListSize(orbitalCount, orbPos);
    }

    void EnsureListSize<T>(int count, List<T> list)
    {
        while (list.Count < count) list.Add(default);
        while (list.Count > count) list.RemoveAt(list.Count - 1);
    }

    void OnDestroy()
    {
        for (int i = 0; i < visuals.Count; i++)
            if (visuals[i]) Destroy(visuals[i].gameObject);
        visuals.Clear();
    }
}
