using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow : Monster
{
    public Yellow() : base(MonsterType.Far) //소괄호를 생성자를 뜻함.
    {
        Debug.Log($"자식 생성자 호출");
    }
    public void Attack()
    {
        Debug.Log("공격");
    }
}
