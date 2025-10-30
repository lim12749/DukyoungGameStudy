using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SimpleProjectile : MonoBehaviour
{
    [Header("Motion")]
    public float speed = 20f;
    public float maxLifetime = 3f;

    [Header("Hit Filter")]
    public LayerMask hitMask;          // 적 레이어만 넣어두면 필터링 쉬움

    [Header("Damage")]
    public float damage = 10f;
    public bool  isCrit = false;       // ← 크리티컬 여부(발사 시 세팅)

    float life;
    Vector3 dir;
    Rigidbody rb;
    Collider col;

    /// <summary>
    /// 발사 초기화. crit=true면 크리티컬 텍스트(빨간색)로 표시됨.
    /// </summary>
    public void Init(float dmg, Vector3 direction, bool crit = false)
    {
        damage = dmg;
        isCrit = crit;
        dir = direction.normalized;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // 트리거 물리 세팅
        rb.useGravity = false;
        rb.isKinematic = true; // 직접 위치 이동
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        col.isTrigger = true;
    }

    void Start()
    {
        if (dir.sqrMagnitude < 1e-6f) dir = transform.forward; // 안전장치
    }

    void Update()
    {
        life += Time.deltaTime;
        if (life >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += dir * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    void OnTriggerEnter(Collider other)
    {
        // 레이어 필터
        if ((hitMask.value & (1 << other.gameObject.layer)) == 0) return;

        // 데미지 적용
        var dmgTarget = other.GetComponentInParent<IDamageable>();
        if (dmgTarget != null)
        {
            dmgTarget.TakeDamage(damage);

            // 데미지 텍스트 스폰 (충돌 지점 근처로 살짝 위)
            Vector3 contact = other.ClosestPoint(transform.position);
            DamageTextSpawner.Instance?.Spawn(contact + Vector3.up * 0.8f, damage, isCrit);
        }

        // 관통 X: 첫 히트에서 파괴
        Destroy(gameObject);
    }
}
