using UnityEngine;

public class RangedBasicAttack : MonoBehaviour, IBasicAttack
{
    [Header("Projectile")]
    public GameObject projectilePrefab;     // ★ 총알 프리팹(SimpleProjectile만 붙어 있어야 함)
    public float projectileSpeed = 20f;
    public float projectileLife  = 3f;
    public LayerMask projectileHitMask;     // 적 레이어 포함

    [Header("Attack")]
    public float baseCooldown = 0.5f;

    [Header("MultiShot")]
    public int   projectileCount = 1;       // 카드로 +1, +2 …
    public float spreadAngleDeg  = 10f;

    GameObject  owner;
    PlayerStats stats;
    float cd;

    public void Initialize(GameObject ownerGO, PlayerStats s)
    {
        owner = ownerGO;
        stats = s;
        cd = 0f;
    }

    void Update()
    {
        if (cd > 0f) cd -= Time.deltaTime;
    }

    public bool TryAttack(Transform target)
    {
        if (cd > 0f || !target || !projectilePrefab || !owner) return false;

        // 쿨타임(쿨감 반영)
        float cdr = stats ? Mathf.Clamp01(stats.cooldownReduction) : 0f;
        cd = Mathf.Max(0.05f, baseCooldown * (1f - cdr));

        // 기준 방향
        Vector3 dir = (target.position - owner.transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 1e-6f) dir = owner.transform.forward;
        dir.Normalize();

        // 멀티샷
        int count = Mathf.Max(1, projectileCount);
        float totalSpread = spreadAngleDeg * (count - 1);
        float startYaw = -totalSpread * 0.5f;

        // 데미지 계산
        float dmg = stats ? stats.attackDamage : 10f;
        if (stats && Random.value < stats.critChance) dmg *= 2f;

        for (int i = 0; i < count; i++)
        {
            Quaternion rot   = Quaternion.AngleAxis(startYaw + spreadAngleDeg * i, Vector3.up);
            Vector3    sdir  = rot * dir;

            var go = Instantiate(
                projectilePrefab,
                owner.transform.position + Vector3.up * 0.9f,
                Quaternion.LookRotation(sdir)
            );

            // ★ Rigidbody 사용 금지. 발사체 스스로 이동하므로 Init으로 값 주입
            var p = go.GetComponent<SimpleProjectile>();
            if (p)
            {
                p.speed       = projectileSpeed;
                p.maxLifetime = projectileLife;
                p.hitMask     = projectileHitMask;
                p.damage      = dmg;
                p.Init(dmg, sdir);            // ★ 방향·데미지 주입 (필수)
            }
            else
            {
                Debug.LogError("[RangedBasicAttack] SimpleProjectile가 프리팹에 없습니다.");
            }
        }

        return true;
    }

    // 카드 업그레이드용
    public void UpgradeMultiShot(int add)  => projectileCount = Mathf.Max(1, projectileCount + add);
    public void UpgradeProjSpeed(float d)  => projectileSpeed = Mathf.Max(1f, projectileSpeed + d);
    public void UpgradeSpread(float d)     => spreadAngleDeg  = Mathf.Max(0f,  spreadAngleDeg + d);
}
