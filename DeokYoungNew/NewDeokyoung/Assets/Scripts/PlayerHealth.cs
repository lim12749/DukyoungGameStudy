using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public float viewHP; //이 값으로 채력 확인


    protected override void OnEnable()
    {


    }
    public override void OnDamage(float _damage)
    {
        if(!Dead) //사망하지 않은경우
        {
            Debug.Log("죽지는 않았고 데미지를 입습니다.");
        }
        base.OnDamage(_damage); //대미지 처리 실행

        viewHP = Health;
    }

    public void OnTriggerEnter(Collider other)
    {
        //충돌시 실행할 이벤트
    }
}
