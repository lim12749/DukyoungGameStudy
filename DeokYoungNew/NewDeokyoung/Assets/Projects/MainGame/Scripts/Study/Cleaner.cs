using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //액션 사용시 필요
/// <summary>
/// Action 공부용
/// Event Action 액션 타입은 입력과 출력이 없는 메서드를 가리킬 수 있는 델리게이트 이빈다. 
/// 델리게이트는 대리자로 본역되며 메서드를 값으로 할당 받을 수 있는 타입 입니다.
/// Action 타입의 변수에는 void Sample() 처럼 입력과 출력이 없는 메서드를 등록할 수 있습니다.
/// 등록된 메서드는 원하는 시점에 매번 실핼할 수 있습니다.
/// 아래예시를 보겠습니다.
/// </summary>
public class Cleaner : MonoBehaviour
{
    Action OnClean; //액션 정의

    void Start()
    {
        OnClean += CleaningroomA;
        OnClean += CleaningroomB;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnClean();
        }
    }
    private void CleaningroomB()
    {
        Debug.Log("A방 청소");
      
    }

    private void CleaningroomA()
    {
        Debug.Log("B방 청소");
       
    }
}
