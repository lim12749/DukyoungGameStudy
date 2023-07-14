using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//네임스페이스는 코드를 작성시 다른 사용자간 코드를 분리하거나 하나의 기능을 그룹화 할때사용합니다. 
namespace Six { 
    //정적
    public class GlobalScore
    {
        //프로그램 전체에서 하나만 존재 해야합니다.
        //전체 프로그램에서 공유해야하는 변수가 있다면 정적으로 필드를 사용하는게 좋습니다.
        //Static을 사용시 클래스에 귀속 됩니다.
        public static int Score =0; //정적 필드생성 정적(static) 
    }
    public class PlayerState
    {
        //필드 작성
        public int HP;
        public int MP;
        public string PlayerName;

        //생성자 *중요
        //기본 생성자
        public PlayerState(){// () 생성자 의미 
            HP = 100;
            MP = 100;
            PlayerName = "언노운";
        }
        public PlayerState(int _HP, int _MP, string _name) //생성자 구분 지어주는게 좋습니다.
        {
            HP = _HP;
            MP = _MP;
            PlayerName = _name;
        }
       
        //메소드 작성
        public void ViewIINFO()
        {
            GlobalScore.Score++; //증가

            Debug.Log($"{HP}, {MP}, {PlayerName}");
        }
    }

    public class playerDeepState
    {
        public int HP;
        public int MP;

        public playerDeepState DeepCopy() //메소드 (클래스 함수)
        {
            playerDeepState newCopy = new playerDeepState();
            newCopy.HP = this.HP;
            newCopy.MP = this.MP;
            return newCopy; //클래스 타입으로 리턴을 해줘야함 
        }
    }

    public class PlayerInfo : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            PlayerState player0 = new PlayerState();
            player0.ViewIINFO();

            PlayerState player1 = new PlayerState(); //인스턴스화 객체 생성
            player1.HP = 100;
            player1.MP = 50;
            player1.PlayerName = "나_전사";

            player1.ViewIINFO();//메소드를 호출하여 사용합니다.

            PlayerState player2 = new PlayerState(); //객체 생성
            player2.HP = 50;
            player2.MP = 100;
            player2.PlayerName = "나_법사";
            player2.ViewIINFO();

            //스포너 ?랜덤캐릭 셀렉
            PlayerState Hero = new PlayerState(500, 500, "영웅등장");
            Hero.ViewIINFO();

            Debug.Log($"{GlobalScore.Score}");

            //-----------------------------------------------------------------//
            //얕은복사 깊은복사

            PlayerState SampleA = new PlayerState();
            SampleA.HP = 100;
            SampleA.MP = 200;

            PlayerState SampleB = SampleA;
            SampleB.MP = 500;

            Debug.Log($"{SampleA.HP} {SampleA.MP}"); // 100 200
            Debug.Log($"{SampleB.HP} {SampleB.MP}"); // 100 500


            //깊은 복사 과정
            playerDeepState SampleDA = new playerDeepState();
            SampleDA.HP = 100;
            SampleDA.MP = 200;

            playerDeepState SampleDB = SampleDA.DeepCopy(); 
            SampleDB.MP = 500;

            Debug.Log($"{SampleDA.HP} {SampleDA.MP}"); // 100 200
            Debug.Log($"{SampleDB.HP} {SampleDB.MP}"); // 100 500

        }
    }
}