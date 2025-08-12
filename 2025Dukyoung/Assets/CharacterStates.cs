using UnityEngine;

/// <summary>
/// 캐릭터 기본 스텟
/// </summary>
[System.Serializable] //공유 데이터
public class CharacterStats 
{
    public int maxHP = 100; //최대 체력
    public int currentHP = 100; //현재 체력
    public int attackPower =10; //공격력

    
    public void ApplyLevelBonus(int level)
    {
        maxHP += level * 10;
        attackPower += level * 2;
    }
}
