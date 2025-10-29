using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SimpleProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float maxLifetime = 3f;
    public LayerMask hitMask;   // 적 레이어만 넣어두면 필터링 쉬움
    public float damage = 10f;

    float life;
    Vector3 dir;
    Rigidbody rb;
    Collider col;

    public void Init(float dmg, Vector3 direction)
    {
        damage = dmg;
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
        if (life >= maxLifetime) { Destroy(gameObject); return; }

        transform.position += dir * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    void OnTriggerEnter(Collider other)
    {
        // 레이어 필터
        if ((hitMask.value & (1 << other.gameObject.layer)) == 0) return;

        var d = other.GetComponentInParent<IDamageable>();
        if (d != null) d.TakeDamage(damage);

        Destroy(gameObject); // 관통 X: 첫 히트에서 파괴
    }
}
