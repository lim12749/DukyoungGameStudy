using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seven{
    public class SowrdMan : PlayerBace
    {
        //기본 생성 클래스를 객체를 생성할때 생성과 동시에 부모에 있는 생성자를 통해 클래스를 초기화 하고 리턴 합니다.
        //생성자는 클래스 이름이랑 같고 리턴이 없으면 생성자
        public SowrdMan() : base(PlayerType.SowrdMan) //생성자에서 부모생성자 호출
        {
            //type = PlayerType.SowrdMan; //우리가 이렇게 새로 플레이어를 만들면 문제가 생길 수 있음
            SetInfo(100, 10);   
        }
        public void Attak()
        {
            Debug.Log("공격!");
        }
        public void Move()
        {
            Debug.Log("이동!");
        }

        //스킬 구현부 만들어도 좋음
    }
}
