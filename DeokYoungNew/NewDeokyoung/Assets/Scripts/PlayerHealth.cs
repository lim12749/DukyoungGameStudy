using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public float viewHP; //이 값으로 채력 확인
    [Header("Audio")]
    public AudioClip deathCilp; //사망 소리
    public AudioClip hitClip;

    private AudioSource playerAudioPlayer;

    [SerializeField] private PLayerController pLayerController;//플레이어가 움직이는 클래스
    [SerializeField] private FindTargetFOV fOV; //플레이어의 공격 모션
    [SerializeField] private PlayerAnimator Playeranim;

    private void Awake()
    {
        //as
        pLayerController = GetComponent<PLayerController>();
        //Fov는 자식이니까 수동으로
        Playeranim = GetComponent<PlayerAnimator>();
    }
    protected override void OnEnable()
    {
        base.OnEnable(); //livingEntity의 OnEnable()를 실행하고 상태를 초기화함

        viewHP = Health;

        //조작 컴포넌트 활성화
        pLayerController.enabled = true;
        Playeranim.enabled = true;
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
    public override void Die()
    {
        base.Die();//사망처리 실행

        //사망시 소리나 체력 비활성화등 진행

        Playeranim.DieAnim(); //죽을때 애니메이션 실행

        pLayerController.enabled = false;
        Playeranim.enabled = false; 
    }
    public void OnTriggerEnter(Collider other)
    {
        //충돌시 실행할 이벤트
    }
}
