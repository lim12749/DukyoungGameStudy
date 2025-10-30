// SentryPassive.cs
using UnityEngine;

public class SentryPassive : MonoBehaviour, IPassiveAttack
{
    [Header("Refs")]
    public GameObject projectilePrefab;
    public LayerMask enemyMask;

    [Header("Rule")]
    public float interval = 0.8f;
    public float range    = 10f;
    public float projSpeed= 18f;
    public float projLife = 3f;

    [Header("Damage")]
    public float baseDamage   = 6f;
    public float damagePerLvl = 3f;

    public string DisplayName => "Sentry";

    Transform owner;
    PlayerStats statsRef;
    int level = 1;
    float t;

    public void Initialize(GameObject ownerGO, PlayerStats stats)
    {
        owner = ownerGO.transform;
        statsRef = stats;
        transform.SetParent(owner, false);
    }

    public void Upgrade(int delta) { level = Mathf.Max(1, level + delta); }

    void Update()
    {
        t -= Time.deltaTime;
        if (t > 0f) return;

        var target = FindNearest();
        if (!target) { t = interval; return; }

        Fire(target.position);
        float cdr = statsRef ? Mathf.Clamp01(statsRef.cooldownReduction) : 0f;
        t = Mathf.Max(0.1f, interval * (1f - cdr));
    }

    Transform FindNearest()
    {
        var cols = Physics.OverlapSphere(owner.position, range, enemyMask, QueryTriggerInteraction.Collide);
        float best = float.MaxValue;
        Transform bestT = null;
        for (int i = 0; i < cols.Length; i++)
        {
            float d2 = (cols[i].transform.position - owner.position).sqrMagnitude;
            if (d2 < best) { best = d2; bestT = cols[i].transform; }
        }
        return bestT;
    }

    void Fire(Vector3 targetPos)
    {
        if (!projectilePrefab) return;

        Vector3 dir = (targetPos - owner.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 1e-6f) dir = owner.forward;
        dir.Normalize();

        var go = Instantiate(projectilePrefab, owner.position + Vector3.up * 0.9f, Quaternion.LookRotation(dir));
        var p  = go.GetComponent<SimpleProjectile>();
        if (p)
        {
            p.speed       = projSpeed;
            p.maxLifetime = projLife;
            p.hitMask     = enemyMask;
            p.damage      = CalcDamage();
            p.Init(p.damage, dir);
        }
    }

    float CalcDamage()
    {
        float d = baseDamage + damagePerLvl * (level - 1);
        if (statsRef) d += statsRef.attackDamage * 0.15f; // 공격력 15% 계수 예시
        return d;
    }
}
