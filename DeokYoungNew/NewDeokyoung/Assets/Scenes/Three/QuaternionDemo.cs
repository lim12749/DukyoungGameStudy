using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 쿼터니언은 할게 많다. 물체의 회전을 쿼터니언을 통해서 지정하기때문에 컴포넌트 Rotaion값을 vector3로 지정하는것처럼 보이지만
/// 쿼터니언으로 지정하고 vector3인것처럼 눈속임하는것 유니티에서 복잡한 연산을 지원해주고있다.
/// </summary>
public class QuaternionDemo : MonoBehaviour
{
    public float DemoFloat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetQuaternionEuler();
    }
    void SetQuaternionEuler()
    {
        //에러 쿼터니언은 4원수의 행렬 연산이다. 그래서 아래처럼 우리가 Vector3 연산하듯이 하게된다면 에러가 나게됨
        //transform.rotation = new Vector3(30, 1, 1); 

        ///Vector3를 쿼터니언으로 변경하는법
        /// Quaternion.Euler(Vector3)
        /// Vector3를 매개변수로 넘겨 받아 그 vector3를 쿼터니언 데이터 타입으로 변환해 리턴시켜주는 함수이다.
        /// Euler는 3차원 벡터로 공간의 회전을 표시 할 수 있도록함.
        //1.
        // transform.rotation = Quaternion.Euler(new Vector3(30, 30, 30)*Time.time); //Rotation값이 30,30,30이 됌
        //var T = 30;
        //Debug.Log(T * Time.time);


        ///LookRotation함수
        /// 백터를 매개션수로 넣어주면 그 오브젝트가 매개변수 방향을 처다보게끔 자기 자신이 방향을 회전함.
        //2. 결국 타겟을 계속 바라보게 할 수 있음
        var item = GameObject.Find("Target").GetComponent<Transform>();
        //transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
        //transform.rotation = Quaternion.LookRotation(item.position);
        //이 개념을 조금더 살펴보자면 백터의 뺄셈과 연관이 있다.
        // 목적지 위치 - 출잘지의 위치 = 움직여야할 방향과 거리 벡터 가 나오게된다.

        //Lerp
        //두개의 회전 값을 정하면 그 중간의 적당한 회전 값을 리턴함.
        /*
       Quaternion aRotation = Quaternion.Euler(new Vector3(0, 30, 0)); //단순하게 회전만 시키는 코드
       Quaternion bRotation = Quaternion.Euler(new Vector3(0, 90, 0));

       Quaternion taregetRotation = Quaternion.Lerp(aRotation, bRotation, DemoFloat); //뒤의 f값이 0.5면딱 중간 1이면 b 0이면 a, 0.2 면 a에 조금더 가깝게
       transform.rotation = taregetRotation;
   */
    
    }
}
