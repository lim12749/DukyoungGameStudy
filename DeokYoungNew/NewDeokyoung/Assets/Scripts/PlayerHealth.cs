using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity
{
    //임시로 사용할 내 HP
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더 머리 위에 작게 띄어두고 감소하는 식으로 사용할 예정


    protected override void OnEnable()
    {
        base.OnEnable();    // LivingEntity의 OnEnable() 실행 (상태 초기화)

        healthSlider.gameObject.SetActive(true); //게임이 시작되면 활성화 합니다

        healthSlider.maxValue = StartingHealth; //시작 체력을 최대채력으로 맞춤

        healthSlider.value = Health; //현재 체력은 base에서 셋팅한 초기 채력으로 셋팅함


        // 플레이어 조작을 받는 컴포넌트들 활성화
        //playerMovement.enabled = true; //나중에 죽거나 새로 시작했을때 값을 사용합니다
        //playerShooter.enabled = true;
    }

    //OnDamage는 적이나 내가 공격을 받을경우 체력을 감소시키는 메소드 입니다.
    public override void OnDamage(float _damage)
    {
        if(!Dead) //사망하지 않은경우 효과음 재생
        {
            Debug.Log("죽지는 않았고 데미지를 입습니다.");
        }
        //대미지 처리 실행
        //채력은 Living에 체력이 있고 이것을 상속하여 관리를 함
        base.OnDamage(_damage);

        //체력은 변동됬지만 슬라이더의 시각적으로 보여지는 체력도 변경
        healthSlider.value = Health;

    }
    public override void Die()
    {
        base.Die(); //LivingEntity Die함수 실행 

        // 체력 슬라이더 비활성화 죽었을때 다른 동작을 못하게 막는 용도
        healthSlider.gameObject.SetActive(false);


    }
    public void OnTriggerEnter(Collider other)
    {
        //충돌시 실행할 이벤트 아이템먹엇을때 사용
    }
}
