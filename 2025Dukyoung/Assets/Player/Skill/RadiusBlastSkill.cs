using UnityEngine;

public class RadiusBlastSkill : MonoBehaviour, IRightClickSkill
{
    [Header("Targeting")]
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public float maxCastDistance = 30f;

    [Header("Effect")]
    public float radius = 4f;
    public float damage = 40f;

    [Header("Cooldown")]
    public float baseCooldown = 6f;

    GameObject owner;
    PlayerStats stats;
    float cdTimer;

    public void Initialize(GameObject ownerGO, PlayerStats s)
    {
        owner = ownerGO; stats = s; cdTimer = 0f;
    }

    void Update()
    {
        if (cdTimer > 0f) cdTimer -= Time.deltaTime;
    }

    public bool TryCast()
    {
        if (cdTimer > 0f) return false;
        if (!Camera.main) return false;

        // 마우스 → 바닥 히트 없으면 실패
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, maxCastDistance, groundLayer, QueryTriggerInteraction.Ignore))
            return false;

        Vector3 center = hit.point;

        // 적 탐색
        var hits = Physics.OverlapSphere(center, radius, enemyLayer, QueryTriggerInteraction.Ignore);
        if (hits == null || hits.Length == 0)
        {
            // 범위 내 적이 없으면 시전 실패 처리(쿨타임 소모 X)
            return false;
        }

        // 데미지 적용
        for (int i = 0; i < hits.Length; i++)
        {
            var d = hits[i].GetComponentInParent<IDamageable>();
            if (d != null) d.TakeDamage(damage);
        }

        // (선택적 시각화가 이미 들어있다면 유지)
        RingPulseVFX.Spawn(center, Mathf.Max(0.3f, radius * 0.25f), radius, 0.25f, new Color(1f, 0.95f, 0.2f, 0.9f));

        // 쿨타임 시작
        float cdr = stats ? Mathf.Clamp01(stats.cooldownReduction) : 0f;
        cdTimer = Mathf.Max(0.05f, baseCooldown * (1f - cdr));
        return true;
    }
}
