using UnityEngine;
using System.Collections;
public class ShooterEnemy : BaseEnemy
{
    public GameObject bulletPrefab;     // 총알 프리팹
    public Transform firePoint;         // 총알 발사 위치
    public float bulletSpeed = 10f;
    public Animator animator;
    public float rotSpeed = 5f;
    public float attackStartRange = 2f;
    public float attackStopRange = 2f;
    private bool isDead = false;
    protected override void TrackAndAttack()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        // 시야 내 방향 체크
        Vector3 toTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, toTarget);
        float angleToTarget = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (distance > attackStartRange)
        {
            agent.SetDestination(target.position);
        }
        else if (distance < attackStopRange)
        {
            agent.ResetPath();
        }
        else
        {
            agent.ResetPath();

            // ▶️ 타겟이 정면 60도 이내에 있을 때만 공격
            if (angleToTarget < 60f)
            {
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;
                    Attack();
                }
            }
            else
            {
                // 타겟이 등 뒤에 있으면 회전만
                RotateTowardsTarget();
            }
        }
    }
    private void RotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f; // 수평 회전만

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
        }
    }
    protected override void Attack()
    {
        if (target == null || bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("⚠️ 공격 실패: 참조 누락");
            return;
        }

        // 총알 생성 및 발사
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 dir = (target.position - firePoint.position).normalized;
        bullet.GetComponent<Rigidbody>().linearVelocity = dir * bulletSpeed;

        // 공격 애니메이션 실행
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        Debug.Log($"{gameObject.name}이 총을 발사함!");
    }
protected override void Die()
{
    isDead = true;

    // 애니메이션 실행
    if (animator != null)
    {
        animator.SetTrigger("Die");
    }

    // 움직임 멈춤
    if (agent != null)
    {
        agent.ResetPath();
        agent.isStopped = true;
    }

    // 총알 발사도 방지되도록
    target = null;

    // 애니메이션 끝날 시간 기다렸다가 삭제
    StartCoroutine(DestroyAfterDelay(2f)); // ← 애니메이션 길이에 맞게 조절
}

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
         base.Die(); 
    }
}
