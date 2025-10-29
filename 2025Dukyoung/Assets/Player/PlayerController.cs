using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    public PlayerStats stats;         // 이동속도/공격력 등
    public LayerMask enemyLayer;      // 적 레이어

    [Header("Movement")]
    public bool useAxis = true;       // true: Horizontal/Vertical, false: WASD 개별키
    public float rotationLerp = 12f;  // 바라보기 보간 속도

    [Header("Auto Attack")]
    public float attackRange = 3f;          // 자동 공격 반경
    public float targetRefreshInterval = .2f;
    public float onHitDebugDuration = .15f; // 디버그 라인 표시 시간(0=표시안함)

    [Header("Basic Attack (Strategy)")]
    public MonoBehaviour basicAttackComponent; // IBasicAttack 구현 컴포넌트(RangedBasicAttack 등)
    IBasicAttack basicAttack;

    Rigidbody rb;
    Transform currentTarget;
    float targetTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!stats) stats = GetComponent<PlayerStats>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (basicAttackComponent) basicAttack = (IBasicAttack)basicAttackComponent;
        if (basicAttack != null) basicAttack.Initialize(gameObject, stats);
    }

    void Update()
    {
        Vector3 moveDir = ReadMove();
        Move(moveDir);

        // 주기적으로 타겟 갱신
        targetTimer -= Time.deltaTime;
        if (targetTimer <= 0f)
        {
            targetTimer = targetRefreshInterval;
            currentTarget = FindNearestEnemyInRange();
        }

        // 타겟이 있으면 타겟을, 없으면 이동방향을 바라보기
        FaceDirection(moveDir, currentTarget);

        // 자동 기본공격 시도(IBasicAttack에 위임)
        if (currentTarget != null && basicAttack != null)
        {
            float d2 = (currentTarget.position - transform.position).sqrMagnitude;
            if (d2 <= attackRange * attackRange)
            {
                if (basicAttack.TryAttack(currentTarget) && onHitDebugDuration > 0f)
                    Debug.DrawLine(transform.position + Vector3.up, currentTarget.position + Vector3.up, Color.red, onHitDebugDuration);
            }
        }
    }

    Vector3 ReadMove()
    {
        if (useAxis)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 dir = new Vector3(x, 0f, z);
            return dir.sqrMagnitude > 1f ? dir.normalized : dir;
        }
        else
        {
            float x = 0f, z = 0f;
            if (Input.GetKey(KeyCode.A)) x -= 1f;
            if (Input.GetKey(KeyCode.D)) x += 1f;
            if (Input.GetKey(KeyCode.S)) z -= 1f;
            if (Input.GetKey(KeyCode.W)) z += 1f;
            Vector3 dir = new Vector3(x, 0f, z);
            return dir.sqrMagnitude > 1f ? dir.normalized : dir;
        }
    }

    void Move(Vector3 dir)
    {
        float speed = stats ? stats.moveSpeed : 6f;
        Vector3 delta = dir * speed * Time.deltaTime;
        rb.MovePosition(rb.position + delta);
    }

    void FaceDirection(Vector3 moveDir, Transform target)
    {
        Vector3 lookDir = Vector3.zero;

        if (target != null)
        {
            lookDir = (target.position - transform.position);
            lookDir.y = 0f;
        }
        else if (moveDir.sqrMagnitude > 0.0001f)
        {
            lookDir = moveDir;
        }

        if (lookDir.sqrMagnitude > 0.0001f)
        {
            Quaternion q = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, rotationLerp * Time.deltaTime);
        }
    }

    Transform FindNearestEnemyInRange()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, attackRange, enemyLayer, QueryTriggerInteraction.Ignore);
        float bestD2 = float.MaxValue;
        Transform best = null;

        for (int i = 0; i < cols.Length; i++)
        {
            Transform t = cols[i].transform;
            float d2 = (t.position - transform.position).sqrMagnitude;
            if (d2 < bestD2) { bestD2 = d2; best = t; }
        }
        return best;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, .4f, .2f, .35f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
