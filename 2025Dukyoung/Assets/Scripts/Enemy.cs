using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour 
{
    [Header("Target")]
    public Transform target;            // 플레이어 Transform
    public string targetTag = "Player"; // ▶ 기본 타겟 태그를 Player로

    [Header("Nav Settings")]
    public float moveSpeed = 3.5f;
    public float acceleration = 8f;
    public float angularSpeed = 120f;

    [Header("Repath")]
    public float repathInterval = 0.2f; // 타겟 재설정 주기(초)

    [Header("Reward")]
    public int expOnDeath = 3;

    NavMeshAgent agent;
    float repathTimer;
    float refindTimer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
        agent.autoBraking = true;
        agent.autoRepath = true; // 플레이어가 움직이므로 켜둠
        ApplyNavStats();
    }

    void Start()
    {
        if (!target)
        {
            var tObj = GameObject.FindGameObjectWithTag(targetTag);
            if (tObj) target = tObj.transform;
        }

        if (target && agent.isOnNavMesh)
            agent.SetDestination(target.position);
    }

    void Update()
    {
        // 런타임에서 Nav 설정값 실시간 반영(인스펙터 값 변경 시)
        ApplyNavStats();

        // 타겟이 사라졌거나 아직 못찾았으면 주기적으로 재탐색
        if (!target)
        {
            refindTimer -= Time.deltaTime;
            if (refindTimer <= 0f)
            {
                refindTimer = 0.5f;
                var tObj = GameObject.FindGameObjectWithTag(targetTag);
                if (tObj) target = tObj.transform;
            }
            return;
        }

        if (!agent.isOnNavMesh) return;

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            repathTimer = repathInterval;
            agent.SetDestination(target.position);
        }
    }

    void ApplyNavStats()
    {
        if (!agent) return;
        agent.speed = moveSpeed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
    }

    public void Die()
    {
        PlayerExperience.Instance?.AddExp(expOnDeath);
        Destroy(gameObject);
    }
}
