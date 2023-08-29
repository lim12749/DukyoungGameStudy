using UnityEngine;

public class SampleClass
{ 
    //C++에서는 프로퍼티 개념이 아니라서 겟터 셋터를 함수(메소드 형태로)구현해줘야한다.
    //은닉성도 살리면서 변수값을 사용하고 싶을때 사용합니다.
    protected int hp;

    //프로퍼티
    public int Hp
    {
        get { return hp;} //값을 꺼내올때 사용
        set
        {
            this.hp = value;
        } //기본으로 value를 사용함
    }

    private int noSetHp = 1000; //이런식으로 초기값이 셋팅되어 get으로값만 빼올수 있게끔 가능
    public int NoSetHp
    {
        get { return noSetHp; }
    }
    //셋만 막아보기
    private int attack;
    public int Attack
    {
        get { return attack;}
        private set { attack = value; }
    }
    
    //자동구현 프로퍼티
    //필드를 위에서는 추가하고 사용하는데 더 단축한다.
    public int AutoProperty { get; set; } //위에 겟터 셋터를 전부 만들어지게된다.
    
    //버전업 c#에서 사용하는 7.0 이상 버전
    public int UpgradeProperty { get; set; } = 100; //선언시 초기화 가능 
    //ㅇㅣ 두과정을 프로퍼티 문법으로 사용함
    /*
    public int GetHP()
    {
        return hp;
    }
    public void SetHP(int _hp)
    {
        this.hp = _hp;
    }*/
}
public class DemoProperty : MonoBehaviour
{
    private void Start()
    {
        SampleClass sampleClass = new SampleClass();
        sampleClass.Hp = 100;

        int viewhp = sampleClass.Hp;
        
        Debug.Log(viewhp);
    }
}
