using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Vector3 velocity; //속도
    public bool isWork { get; set; }

    private Animator myAnimator; //애니메이터 클래스를 가져옵니다

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        //하위, 자식오브젝트의 클래스를 참조
        myAnimator = GetComponentInChildren<Animator>();
        isWork = false;

        //참고 외부에 있는 클래스를 참조할때 사용
        //var find = GameObject.Find("오브젝트이름").GetComponent<클래스>();
    }

    private void Update()
    {
        //애니메이션을 실행 할때 set은 값을 설정할때 사용bool 파라미터 타입 입니다.
        //"IsRun" 애니메이션을 컨트롤하는 파라미터의 이름이고
        //isWork는 컨트롤하는 변수입니다.
        myAnimator.SetBool("IsRun", isWork);
    }

    public void MoveMnetnimationSet(Vector3 _velocity)
    {
        velocity = _velocity; //움직이고 있다
        isWork = velocity != Vector3.zero;
    }
    public void AttackAnimSet() //공격 애니메이션 실행
    {
        myAnimator.SetTrigger("attack");
    }
    public void DieAnim() //죽는 애니메이션 실행
    {
        myAnimator.SetTrigger("Die");
    }

}
