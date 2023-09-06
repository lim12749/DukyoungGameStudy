using UnityEngine;
using System; //이벤트 사용
/// <summary>
/// Player Monster share
/// </summary>
public class LivingEntity : MonoBehaviour
{

    public float StartingHealth = 100f; //Setting Valuse

    //Auto-implemented properties 
    public float Health { get; protected set; } //Now Hp Value

    public bool Dead { get; protected set; } //Now Living

    //델리게이트 :  입력과 출력이 없는 메서드를 가르킬수 있다.
    //등록된 메서드는 원하는 시점에 매번 실행 할 수 있다.
    public event Action OnDeath; // 사망시 발동할 이벤트

    //플레이시 부모 클래스에서 먼저 OnEnable가 실행됩니다
    //Start 함수 이전에 호출됩니다. 게임 플레이 또는 리셋시 한번 실행합니다
    //SetActive로도 활성화 됩니다,  생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable()
    {
        Dead = false; //생성, 살아날때 상태를 죽지 않음으로 변경

        Health = StartingHealth; //시작 체력으로 이니셜라이즈!(초기화)
    }
    //데미지를 적용하는 함수
    public virtual void OnDamage(float damage )
    {
        Health -= damage; // 데미지 적용

        if(Health<=0 &&!Dead)
        {
            Die(); //사망 처리 함수 실행
        }
    }
    // 체력을 회복하는 기능
    public virtual void RestoreHealth(float newHealth)
    {
        if (Dead)
        {
            // 이미 사망한 경우 체력을 회복할 수 없음
            return;
        }

        // 체력 추가
        Health += newHealth;
    }
    // 사망 처리
    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (OnDeath != null)
        {
            OnDeath(); //함수를 등록해두고 일괄적으로 실행 합니다
        }

        // 사망 상태를 참으로 변경
        Dead = true;
    }
}
