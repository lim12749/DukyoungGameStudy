using UnityEngine;
using UnityEngine.AI;
public abstract class BaseEnemy : MonoBehaviour
{
[Header("공통 스탯")]
    public int maxHP = 100;
    protected int currentHP;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    protected float lastAttackTime;
    public GameObject expOrbPrefab;
    
    [Header("AI 컴포넌트")]
    protected NavMeshAgent agent;
    public Transform target;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
    }

    protected virtual void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // 이동
        if (distance > attackRange)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack();
            }
        }
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"{gameObject.name}이 피해를 입음: {amount}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
            Debug.Log($"{gameObject.name} 사망");

            // 경험치 구슬 생성
            if (expOrbPrefab != null && target != null)
            {
                GameObject orb = Instantiate(expOrbPrefab, transform.position + Vector3.up, Quaternion.identity);
                //orb.GetComponent<ExpOrb>()?.SetTarget(target); // 플레이어 따라가게
            }

            Destroy(gameObject);
    }

    // 자식 클래스가 구현할 공격 함수
    protected abstract void Attack();
}
