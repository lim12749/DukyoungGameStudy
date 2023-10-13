using System;
using UnityEngine;

// 대리자 사용 빈도가 높은 문법중 하나
/// 코드를 항상 순차적으로 만들었다.
/// 업체 사장이 있다 업체사장과 연락하기 위해선 비서와 연락쳐를 전달하고 한다 델리게이트는 비서와 같은역할이다.

namespace GW_Study
{
    public class DemoDelegate : MonoBehaviour
    {
        //읽는법 (함수가 아니라 형식이다) 자체가 하나의 형식으로 이해해야한다
        //delegate -> 형식은 형식인데 함수 자체를 인자로 넘겨주는 그런 형식이다.
        //반환은 int 입력() void 전체이름 OnClicked가 delegate 형식의 이름
        //[형식선언][반환][형식이름][입력]
        delegate int OnClicked();

        delegate int Sample();
        //위 delegate형식과 같은 메소드 생성
        int Method()
        {
            Debug.Log("Hello Delegate");
            return 0;
        }
        int MethodTwo()
        {
            Debug.Log("Hello Delegate2");
            return 0;
        }
        private void Start()
        {
           // void ButtonPressed();
           Sample sample = new Sample(Method); //함수 자체를 전달함
           sample += MethodTwo;//델레게이트 체이닝 체이닝은 여러가지 함수를 같이 호출할 수 도 있다.
        }

        void SamplePlaye(Sample _sample)
        {
            _sample();// 호출
        }
        //델레게이트는 체이닝도 가능하다
        
        //함수를 넘기고 내부에서 함수를 실행하는 방식을 콜백 방식이라고 한다.
        void ButtonPressed(/*함수 자체를 인자로 넘겨주고  */)
        {
            //함수를 호출
        }
    }
}