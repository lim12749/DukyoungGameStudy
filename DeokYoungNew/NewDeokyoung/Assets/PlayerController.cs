using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 스크립트를 가진 오브젝트는 방향키, wasd키를 이용하요 월드좌표에서 움직일 수 있습니다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    //방향 설정
    private Vector3 viewForward; //정면 기준
    private Vector3 viewRight; //오른쪽 기준
    private Vector3 direction; //최종 방향

    //캐릭터 움직이기위한 클래스
    private CharacterController myCharacterController;
    //캐릭터 애니메이션을 컨트롤하기위한 클래스
    private PlayerAnimator myPlayerAnim; //애니메이션 동작을 위한 클래스
    [Header("Setteing")] //인스펙터뷰에 타이틀
    public float moveSpeed; //플레이어 이동속도
    private void Awake() //게임이 실행했을때 단한번 호출
    {
        Init(); //게임이 시작하자마자 초기화 해줘야한다
    }
    private void Init() //초기화를 위한 함수
    {
        //방향 정의
        //카메라가 보고있는 정면을 기준점으로 만듬
        viewForward = Camera.main.transform.forward;
        viewForward.y = 0f; //높이가 있으면 안됩니다.
        //방향 벡터의 길이 1로 만들어 줍니다.
        viewForward = Vector3.Normalize(viewForward);
        //Quaternion은 내 백터가 오류가 안나게 하기위함
        //정면기준의 오른쪽을 정의함
        viewRight = Quaternion.Euler(new Vector3(0, 90, 0)) * viewForward;

        //컴포넌트 추가
        //getComponet는 이 클래스를 사용하는곳에 같이 있어야합니다.
        myCharacterController = GetComponent<CharacterController>();
        myPlayerAnim = GetComponent<PlayerAnimator>(); 
    }

    //물리가 들어가지 않은 단순 식
    private void Update()
    {
        PlayerMovement();
    }
    //물리가 들어가는 연산식
    private void FixedUpdate() 
    {
        
    }
    private void PlayerMovement() // Movemnet , Locomotion 
    {
        //오른쪽 키를 입력받는 변수 정의
        Vector3 rightMove = viewRight * Input.GetAxis("Horizontal"); //수평
        //상하 키를 입력바는 변수 정의
        Vector3 forwardMove = viewForward * Input.GetAxis("Vertical"); //수직
        //입력키를 받는 최종 백터정의
        Vector3 finalMovement = forwardMove + rightMove;
        //이동방향을 최종적으로 1로 만들어 줍니다.
        direction = Vector3.Normalize(finalMovement); //

        myPlayerAnim.MoveMnetnimationSet(direction); //애니메이션 값을전달함

        //움직이냐? 
        if(direction != Vector3.zero)
        {
            //플레이어 정면기준으로 움직이게하기
            transform.forward = direction; //방향 설정
            //캐릭터컨트롤러를 이용해서 움직이게 하기
            myCharacterController.Move(direction * moveSpeed* Time.deltaTime);

        }
    }

    
}
