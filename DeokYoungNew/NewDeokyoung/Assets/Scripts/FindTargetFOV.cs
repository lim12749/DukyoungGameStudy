using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetFOV : MonoBehaviour
{
	public PlayerAnimator playerAnim;
	public float Radius;

    public float viewAngle;
	public Vector3 doAttackAngle;
	public GameObject attackTarget; //공격대상
	public LayerMask targetMask;

    public float FindTargetDelayTime;
    public float attackDelaySpeed; //공격 속도

    public List<Transform> visibleTargets = new List<Transform>();
    public Collider[] targetsInViewRadius; //콜라이더에 들어온 객ㄴ
    public float closest_dst;

	public GameObject ArrowObject;
	public Transform InstancPos;

    private void Start()
    {
		StartCoroutine(FindTargetDelay());
		StartCoroutine(AutoAttack());
    }

    IEnumerator FindTargetDelay()
    {
        while(true)
        {
            yield return new WaitForSeconds(FindTargetDelayTime);
            FindTarget();
        }
    }
	IEnumerator AutoAttack()
    {
		while(true)
        {	
			yield return new WaitForSeconds(attackDelaySpeed);
			if(attackTarget)
            {
				playerAnim.AttackAnimStart();
				Attack();
            }
			else
            {
				//Debug.Log("타겟이 없음");
				continue;
            }
        }
    }
	void Attack()
    {
		var item = Instantiate(ArrowObject, InstancPos.position,Quaternion.identity).GetComponent<Arrow>();
		item.SetLocomotion(attackTarget.transform, InstancPos);
	
    }

    void FindTarget()
    {
        targetsInViewRadius = Physics.OverlapSphere(transform.position, Radius, targetMask);

        closest_dst = Radius; //가장가까운거리 = 공격 범위
		visibleTargets.Clear();
		GameObject closest_target = null; //가장가까운 타겟을 여기에 넣고
        int count = 0; //공격리스트 카운트

		for (int i = 0; i < targetsInViewRadius.Length; i++)//들어온 객체 만큼 포문을 돌림
		{
			GameObject swaptarget = targetsInViewRadius[i].gameObject; //게임오브젝트를 넣음
			if (swaptarget.tag != "Monster") continue; //테그에 적이 아니면 넘

			Transform target = swaptarget.transform; //걸러진 게임오브젝트 위치를 저장  배열에 저장된 정보만큼 for문이 돌면서 오브젝트를 호출하는 모습을 보임

			Vector3 dirToTarget = (target.position - transform.position).normalized; //상대방 벡터와 플레이어 백터를 빼면 방향이 나온다 그리고 정규화 1인벡터로 만들기

			float dstToTarget = Vector3.Distance(transform.position, target.position); //실수값으로 내위치와 타겟의 위치의 사이 거리를반환 ( a - b ) 가됨

			//충돌체가 레이어필터(벽)이 참이면 list를 추가하지 않음
			if (Physics.Raycast(transform.position, dirToTarget, dstToTarget)) //타겟이 있을경우 내위치와 백터 방향, 백터 거리, 벽이면
			{
				count++; //들어온 오브젝트가 있다면 카운트를 추가한다
				//Debug.Log("count변수 Counting  " + count);
				visibleTargets.Add(target); //타겟을 리스트에 추가함
				if (dstToTarget < closest_dst)
				{
					//Debug.Log("closet_dst값이 참이면");
					doAttackAngle = dirToTarget; //가장가까운 몬스터와의 거리 포지션
					closest_dst = dstToTarget; //거리값
					closest_target = swaptarget; //가까운 타겟은 타겟 배열의 값을 넣음
				}
			}
		}
		if (count == 0)
		{
			attackTarget = null; //공격대상없음
		}
		else
		{
			attackTarget = closest_target;//가장가까운 타겟의 오브젝트를 값대입
			//attackTarget.GetComponent<Monster>().SelectImage.SetActive(true);
		}
	}

}
