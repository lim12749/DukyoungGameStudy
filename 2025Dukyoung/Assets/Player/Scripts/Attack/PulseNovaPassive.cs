using UnityEngine;

public class PulseNovaPassive : MonoBehaviour, IPassiveAttack
{
    [Header("Rule")]
    public float interval = 2.0f;
    public float radius   = 3.0f;
    public float baseDamage = 8f;
    public LayerMask enemyMask;

    [Header("Scaling per Level")]
    public float damagePerLevel = 4f;
    public float radiusPerLevel = 0.2f;
    public float cdrPerLevel    = 0.05f;

    [Header("Visual (optional)")]
    public GameObject ringPrefab;   // 얇은 구체/원형 이펙트 프리팹
    public float ringAlive = 0.25f; // 얼마 동안 보일지

    public string DisplayName => "Pulse Nova";

    Transform owner;
    PlayerStats statsRef;
    int level = 1;
    float timer;

    public void Initialize(GameObject ownerGO, PlayerStats stats)
    {
        owner = ownerGO.transform;
        statsRef = stats;
        transform.SetParent(owner, false);
    }

    public void Upgrade(int levelDelta)
    {
        level = Mathf.Max(1, level + levelDelta);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Fire();
            float cdr = (statsRef ? Mathf.Clamp01(statsRef.cooldownReduction) : 0f);
            float localCdr = 1f - Mathf.Clamp01(cdrPerLevel * (level - 1));
            timer = Mathf.Max(0.2f, interval * (1f - cdr) * localCdr);
        }
    }

    void Fire()
    {
        float r  = radius + radiusPerLevel * (level - 1);
        float dmg= baseDamage + damagePerLevel * (level - 1);
        if (statsRef) dmg += statsRef.attackDamage * 0.2f;

        var hits = Physics.OverlapSphere(owner.position, r, enemyMask, QueryTriggerInteraction.Collide);
        foreach (var h in hits)
        {
          
            var d = h.GetComponentInParent<IDamageable>();
            if (d != null) d.TakeDamage(dmg);
                    DamageTextSpawner.Instance?.Spawn(h.transform.position + Vector3.up * 1f, dmg, false);
        }

        // 시각화(옵션)
        if (ringPrefab)
        {
            var ring = Instantiate(ringPrefab, owner.position, Quaternion.identity);
            ring.transform.localScale = Vector3.one * (r * 2f); // 지름 기준
            Destroy(ring, ringAlive);
        }
    }
}
