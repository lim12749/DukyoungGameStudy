using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenPlayerBace
{
    public int id;
    public int HP;
    public int MP;
    public int Attack;

    //생성자
    public SevenPlayerBace()
    {
        id = 1;
        HP = 100;
        MP = 1000;
        Attack = 20;
        Debug.Log("Player BaseClass 기본 생성자 호출 ");
    }
    public SevenPlayerBace(int _hp)
    {
        id = 1;
        HP = _hp;
        MP = 1000;
        Attack = 20;
        Debug.Log("Player HP 기본 생성자 호출");
    }

    public void BaseMethod()
    {
        Debug.Log("베이스 메소드 호출 ");
    }
}
