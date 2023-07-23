using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Six
{
    public class SixInventory : MonoBehaviour
    {
        public class Stuff
        {
            public int projectileA;
            public int projectileB;
            public int projectileC;

            //Construector
            //생성자를 사용하면 프로그래머가 기본값을 설정하고 인스턴스화를 제한하고 유연하며 읽기 쉬운 코드를 작성 할 수 ㅇㅆ습니다
            public Stuff() //기본 생성자
            {
                projectileA = 1;
                projectileB = 1;
                projectileC = 1;
            }

        }

        public Stuff myStuff = new Stuff(); //create instance(an Object) 
        //클래스에 데이터 유형을 부여하면 클래스의 이름이 들어가고 new 라는 키워드를 지정합니다. 그리고 다시 클래스 이름을 사용 합니다.
        //끝에 ()는 생성자를 사용한다는 의미입니다. 클래스나 구조체가 생성될때(인스턴스) 될때 마다 생성자가 호출 됩니다.

        //
        

        private void Start()
        {
            Debug.Log(myStuff.projectileA);
        }
    }
}
