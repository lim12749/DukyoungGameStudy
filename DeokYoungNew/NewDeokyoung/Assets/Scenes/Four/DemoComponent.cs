using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoComponent : MonoBehaviour
{
    Rigidbody rb; //물리 기능을 가진 컴포넌트 선언!! 변수처럼 선언을 하여야지 사용가능함

    void Start()
    {
        //게임이 시작될때 초기화 과정을통해 컴포넌트 연결 수행과정을 거침
        //연결을 할때 누구의 기능을 가져와서 동작시킬것인지 명확하게 생각을하고 연결을 해야합니다.
        rb = gameObject.transform.GetComponent<Rigidbody>(); //이 클래스를 가지고있는 게임 오브젝트의 컴포넌트를 연결합니다.

        rb.isKinematic = false;// .연산자를 통해 아래있는 프로퍼티, 함수등 기능을 확인하고 사용 할 수 있습니다.

        //만약 컴포넌트가 없는경우
        GameObject a = transform.gameObject; //추가할 게임오브젝트 컨테이너를 연결하고
        a.AddComponent<Joint>();  //AddComponet<추가할 기능>(); 사용하여 추가해줍니다.
    }
}
