using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 타겟을 찾고, 여러개 몬스터를 찾고 그 사이에서 나와 제일 거리가 가까운 몬스터를 공격할 수 있게 합니다.
/// </summary>
public class FindTargetFOV : MonoBehaviour
{
    [Header("클래스 기능 셋팅")]
    public PlayerAnimator playerAnim; //공격애니메이션 실행 하기 위함

    [Header("몬스터 셋팅")]
    //반지름 : 구체를 만들고 그 찾는 범위의 크기를 조절할때 사용
    public float Redius; //탐색 범위
    public GameObject AttackTarget; //찾은공격 대상
    public LayerMask targetMask; //식별 하기위 사용  비트연산 예 중독 : 0001  + 화상 0010 = 0011  
 
    public float FindTargetDelayTime;   //찾는시간을 지연을 두고 탐색하게 합니다
    public float attackDelaySpeed; //공격 지연시간

    public List<Transform> visibleTargets = new List<Transform>();
    public Collider[] targetsInViewRadisu; //콜라이더 안에들어온 몬스터
    public float closet_dst;//거리

    public GameObject ArrowObj; //화살
    public Transform InstancePos; //화살 소환 위치

    [Header("값 확인용")]
    public Vector3 doAttackAngle; //

    private void Start()
    {
        //코루틴 기능을 실행할때 사용하는 함수(실행할 코루틴 함수이름)
        StartCoroutine(FindTargetDelay());
        
    }
    IEnumerator FindTargetDelay() //함수 코루틴
    {
        while (true)
        {
            //잠깐 양보하는 시간을 줍니다 (지연시간)
            yield return new WaitForSeconds(FindTargetDelayTime);
            FindTarget();
        }
    }
    public void FindTarget()
    {
        //물리 구체를 만듭니다(중심점, 반지름, 분류레이어)
        targetsInViewRadisu = Physics.OverlapSphere(transform.position, Redius, targetMask);

        closet_dst = Redius;//가장 가까운 거리 = 공격 범위
        visibleTargets.Clear(); //분류된 몬스터의 리스트에 넣기전에 클리어 해줍니다.
        GameObject cloeSet_target = null; //가장 가까운 몬스터를 넣어 주
        int count = 0;//공격 대상리스트 카운트용

        //탐색한배열 만큼 반복 해줍니다
        for(int i=0; i<targetsInViewRadisu.Length; i++)
        {
            //값을 빼서 옮기는 과정
            GameObject swapTarget = targetsInViewRadisu[i].gameObject;
            if (swapTarget.tag != "Monster")
                continue; //다음 인댁스 또는 다시 실행하라
            
            Transform target = swapTarget.transform;
            //타겟과 나의 거리 방향을 뽑아 냅니다
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            // 방향에서의 상대방과의 거리 값(수치)
            float distanceToTarget = Vector3.Distance(transform.position, dirToTarget);

            //물리 레이저를 발사합니다(출발 위치, 도착 위치, 레이저 길이값, out 충돌체 정보)
            if(Physics.Raycast(transform.position,dirToTarget,distanceToTarget))
            {
                count++;
                visibleTargets.Add(target); //타겟을 리스트에 넣어줌
                doAttackAngle = dirToTarget;
                closet_dst = distanceToTarget;
                cloeSet_target = swapTarget; 
            }
        }
        if(count == 0)
        {
            AttackTarget = null; //공격 대상이 없음
        }
        else
        {
            AttackTarget = cloeSet_target; //가장 가까운 타겟의 오브젝트 값을 대입
        }
    }
}
