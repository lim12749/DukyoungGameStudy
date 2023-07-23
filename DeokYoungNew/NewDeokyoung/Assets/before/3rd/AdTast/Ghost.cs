using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterType { None = 0, Almost =1, Far = 2 }
public class Monster : MonoBehaviour //Base / 부모 클래스
{
    //은닉성 ? 정보가 노출되지 않게 막아주면서 우리가 사용할수 있게 만들어야함.
    //  private      protected        public
    public MonsterType monsterType; //누구나 접근이 가능
    private int Hp; //정보를 내 클래스외 접근을 못하게 막음
    private int Attack; //상속받은 클래스까지만 접근이 가능
    //프로퍼티
    //public int MP { get; set; } //자동구현 프로퍼티
    private int TotalMP; //실제 저장되는 값
    public int ViewMP //다른곳에서 사용할때 보여지는 변수 
    {
        get //getter는 값을 전달할때
        {
            return TotalMP;
        }
        set //값을 할당할때
        {
            TotalMP = value; //valuse 는 외부에서 들어오는 값을 말함
        }
    }
   

    //생성자 ( 생성자는 클래스를 인스턴스화 했을때 호출 )
    public Monster(MonsterType type)
    {
        monsterType = type;
        Debug.Log("베이스 클래스");
    }
    public void SetInfo(int _hp, int _attack) //외부에서 값을 셋팅할때 사용할 메소드
    {
        this.Hp = _hp; //this 키워드는 나 자신을 가르킵니다.
        this.Attack = _attack;
    }
    //공통적인 기능을 상속부분에서 만듭니다.
    public int GetHP() { return Hp; }  
    public int GetAttack() { return Attack; }
    public bool IsDie() { return Hp <= 0; }
    public void IsDamage(int _damage)
    {
        Hp = Hp - _damage; 
        if (Hp < 0)
            Hp = 0;
    }
    public Monster()  //기본 생성자
    {
        Debug.Log("Base 기본 생성자 호출");
    }
}
//상속 사용법 클래스 옆에 : [클래스이름]
public class Ghost : Monster // 상속을 받은 / 자식 클래스 / chaild / 파생클래스
{
    //움직임 셋팅
    //스킬 셋팅
    //기능 셋팅
    public Ghost() : base(MonsterType.Almost) //소괄호를 생성자를 뜻함.
    {
        Debug.Log($"자식 생성자 호출");
    }
    public void AttackGhost()
    {
        Debug.Log("고스트 공격합니다");
    }
    public void Move()
    {
        Debug.Log("고스트 움직입니다.");
    }
}
