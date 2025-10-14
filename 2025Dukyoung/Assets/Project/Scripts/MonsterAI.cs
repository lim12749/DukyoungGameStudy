using UnityEngine;
using UnityEngine.AI;
public class MonsterAI : MonoBehaviour
{
    public Transform target; // 추적할 대상 (플레이어)
    public float attackDistance = 2f; // 공격 거리
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackDistance)
        {
            Attack();
        }
        else
        {
            Chase();
        }
    }

    void Chase() //
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
        Debug.Log("추적 중...");
    }

    void Attack()
    {
        agent.isStopped = true;
        Debug.Log("🔴 공격 중!");
        // TODO: 애니메이션, 데미지 처리 등 실제 공격 로직 추가 가능
    }
}
