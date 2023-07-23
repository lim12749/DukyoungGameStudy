using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//정적 필드를 프로그램에 전체에 하나만 존재해야만 합니다.
//정적 필드를 사용시에는 전체에 공유해야하는 변수가 있다면 정적 필드를 이용합니다. 예를 들어 점수 카운트

namespace Six{
    public class Global{
        public static int count = 0;
    }
    public class DemoA {
        public DemoA() // Aclass instance start
        {
            Global.count++;
        }
    }
    public class DemoB {
        public DemoB()// Bclass instance start
        {
            Global.count++;
        }
    }
    public class StaticDemo : MonoBehaviour{

        private void Start()
        {
            print(Global.count);

            new DemoA(); //instance
            new DemoB();

            print(Global.count);
            
        }
    }
}
