using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어의 기본이 되는 클래스와 필드를 작성하고 이를 상속합니다.
/// 공통적으로 사용하게 되는것을 만들어 코드의 중복을 막아줍니다.
/// </summary>
namespace Seven{
    public enum PlayerType
    {
        None,SowrdMan,Archer,Mage
    }
    public class PlayerBace : MonoBehaviour
    {
        [SerializeField] PlayerType type;
        public int HP =0;
        public int Attack =0;

        protected PlayerBace(PlayerType _type)
        {
            this.type = _type;
        }
        public void SetInfo(int _hp, int _attack)
        {
            this.HP = _hp;
            this.Attack = _attack;
        }
        //외부에서 호출할수있게
        public int GetHp() { return HP; }
        public int GetAttack() { return Attack; }

        public bool IsDead() { return HP <= 0; }
        public void OnDamage(int damage)
        {
            HP -= damage;
            if (HP < 0)
                HP = 0;
        }

    }
}
