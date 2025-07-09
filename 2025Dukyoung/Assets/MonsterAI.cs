using UnityEngine;
using UnityEngine.AI;
public class MonsterAI : MonoBehaviour
{
    public Transform target; // 추적할 플레이어
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
