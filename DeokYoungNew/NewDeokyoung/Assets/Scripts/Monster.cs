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
    public LayerMask whatIsTarget; // 추적 대상 레이어

    private bool hasTarget
    {
        get
        {
            if(targetEntity !=null && !targetEntity.Dead)
            {
                return true;
            }
            return false;
        }
    }

    [Header("Attack Pramiter")]
    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점
    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }
    private void Update()
    {
        /*
        //목표 찾는걸 업데이트 해야합니다 
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
        }*/
    }
    private IEnumerator UpdatePath()
    {
        //몬스터가 살아있는 경우 무한루프를 돌립니다.
        while(!Dead)
        {
            if(hasTarget)
            {
                //추적대상이 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else //추적대상이 없으면
            {
                //Ai를 이동 중지 시킵니다.
                pathFinder.isStopped = true;

                //20유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옵니다.
                //단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링 합니다.
                //중심, 반지름 , 레이어 검출
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                for(int i=0; i<colliders.Length;i++)
                {
                    var livingEntity = colliders[i].GetComponent<LivingEntity>(); //컴포넌트 기능 가져와서 검출에 사용

                    //Living 컴포넌트가 존재하고 해당 Living이 살아있는지 체크
                    if (livingEntity != null && !livingEntity.Dead)
                    {
                        //추적대상을 해당 livingEntity로 설정
                        targetEntity = livingEntity;

                        //for문을 즉시 정지
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f); //0.25초씩 지연하고 무한반복을 돌립니다.
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
        if (!Dead && Time.time >= lastAttackTime + timeBetAttack)
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
