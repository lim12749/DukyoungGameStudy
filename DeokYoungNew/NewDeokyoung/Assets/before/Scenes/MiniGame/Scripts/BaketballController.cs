using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaketballController : MonoBehaviour
{
public float MoveSpeed;

public Transform Ball; //플레이어는 공 개체에 대해서 알아야하며 참조합니다.
public Transform Arms; //팔
public Transform PosOverHead;//공을잡고 팔을 드는 상태
public Transform PosDibble; //공을 드리블하는 상태
public Transform Target;
public Transform Pivot;
public bool InBallInHands =true;

public float sin;
private bool IsBallFlying = false;

private float T;
private void Update()
{
    //walking 
    Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")); //이동 방향 결정
    transform.position += direction * MoveSpeed * Time.deltaTime; // 내위치 = 내위치+방향*속도*프레임보정
    transform.LookAt(transform.position + direction);//캐릭터가 걷는 방향을 바라보게 회전시켜줍니다.

    if(InBallInHands)
    {
        if(Input.GetKey(KeyCode.Space)) //누르는동안
        {
            Ball.position = PosOverHead.position; //공의 위치를 PosOverHad 위치로
            Arms.localEulerAngles = Vector3.right * 180; //x축 기준으로 180도로 만듬

            //Look toward the target
            transform.LookAt(Pivot.position);
        }
        else
        {
            //드리블의 특징은 공을 위아래로 튀기는데 있다. 싸인파는 -1 0 1 그래프를 그리는데 ABS 는 절대값으로 -성분을 지워버린다.
            Ball.position = PosDibble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * sin)); //사인파 주기를 만들고 쉐이더에서도 자주 사용;
            Arms.localEulerAngles = Vector3.right * 0;//x축 기준으로 0도로 만듬
        }
    }
    //던지기
    if(Input.GetKeyUp(KeyCode.Space))
    {
        InBallInHands = false;
        IsBallFlying = true;
        T = 0;
    }
    //공날리기
    if(IsBallFlying)
    {
        T += Time.deltaTime;
        float duration = 0.5f;
        float t01 = T / duration;

        Vector3 aPoint =PosOverHead.position; //공은 A위치에서 b위치로 가야함 
        Vector3 bpoint = Target.position;
        Vector3 pos = Vector3.Lerp(aPoint, bpoint, t01); //공이 일자로 날라가는것을 볼 수 있다
        //공은 던지면 포물선을 그리는데 여기서도 수학적인 기능을 하나 사용해보자
        Vector3 arc = Vector3.up *5 * Mathf.Sin(t01* 3.14f); //a여기서 우리는 싸인파를 그릴때 원하는 곡선을 그려야한다. 쉽게 0에서 1인 각 파이를 곱하면된다.. 싸인 공식 참조. t01(시간) * pi

        Ball.position = pos + arc;

        //Momnet when ball
        if(t01 >= 1 ) //공이 1이되었다 /목표위치까지도달하였다
        {
            IsBallFlying = false; //
            Ball.GetComponent<Rigidbody>().isKinematic = false;

        }
    }
}

private void OnTriggerEnter(Collider other)
{
    //공이 손에 없고 공이 날지 않는경우
    if(!InBallInHands && !IsBallFlying)
    {
        InBallInHands = true;
        Ball.GetComponent<Rigidbody>().isKinematic = true;
    }
}
}
