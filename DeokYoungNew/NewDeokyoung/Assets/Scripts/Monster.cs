using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : LivingEntity
{
    public float DieTime;
    public int hp;
    public Animator MonsterAnim;
    public Rigidbody MonsterRigidbody;
    public CapsuleCollider MonsterCollider;
    //public Slider hpUI;
    public UnityEngine.AI.NavMeshAgent pathFinder;
    public GameObject Target;
    public bool isMove = false;
    public bool isRun = false;

    [SerializeField] private LivingEntity targetEntity; // 추적할 대상

    [Header("Attack Pramiter")]
    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점
    private void Start()
    {
        //hpUI.maxValue = hp;
    }
    private void Update()
    {
        pathFinder.SetDestination(Target.transform.position);
        if (Vector3.Distance(Target.transform.position, transform.position) < 3f)
        {
            isRun = false;
            isMove = true;
            MonsterMoveAnim();
        }
        else
        {
            isRun = true;
            isMove = false;
            MonsterRunAnim();
        }
        
    }
    public void MonsterRunAnim()
    {
        MonsterAnim.SetBool("IsRun", isRun);
    }
    public void MonsterMoveAnim()
    {
        //isMove = pathFinder.velocity != Vector3.zero;

        MonsterAnim.SetBool("IsMove", isMove);
    }
    public void SetDamage(int _value)
    {
        hp = hp - _value;
        //hpUI.value = hp;
        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        //is Die play Animator
        MonsterAnim.SetTrigger("IsDie");
        //is Die Off Rigidbody gravity
        MonsterRigidbody.useGravity = false;
        //is Die Off Collider
        MonsterCollider.enabled = false;
        //is Die Off NevMeshAgent
        pathFinder.Stop();

        Destroy(this.gameObject, DieTime);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌");
    }
    private void OnTriggerStay(Collider other)
    {
        // 자신이 사망하지 않았으며,
        // 최근 공격 시점에서 timeBetAttack 이상 시간이 지났다면 공격 가능

        Debug.Log(other.gameObject.name);
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            Debug.Log("d1");
            // 상대방으로부터 LivingEntity 타입을 가져오기 시도

                LivingEntity attackTarget = other.GetComponent<LivingEntity>();
                // 상대방의 LivingEntity가 자신의 추적 대상이라면 공격 실행
                if (attackTarget != null && attackTarget == targetEntity)
                {
                    // 최근 공격 시간을 갱신
                    lastAttackTime = Time.time;
                    Debug.Log("d2");

                    // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                    // Vector3 hitPoint = other.ClosestPoint(transform.position);
                    // Vector3 hitNormal = transform.position - other.transform.position;

                    // 공격 실행
                    attackTarget.OnDamage(damage);
                }
                    
        }
    }
}
