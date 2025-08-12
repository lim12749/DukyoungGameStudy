using UnityEngine;

/// <summary>
/// 플레이어 기본 스텟
/// </summary>
public class PlayerStates : MonoBehaviour, ILevelable, IDamageable
{
    public CharacterStats stats; //캐릭터 기본 스텟
    [SerializeField] private int level = 1; //레벨
    [SerializeField] private int currentExp = 0; //현재 경험치
    [SerializeField] private int expToNextLevel = 10; //다음 레벨까지 필요한 경험치

    /// <summary>
    /// public int Level { get {return level;}} 축약한거 =>는 읽기전용 프로퍼티를 만든거.
    /// </summary>
    public int Level => level; //람다식 표현 레벨  
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;

    public void GainExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        currentExp = 0;
        expToNextLevel += 10;

        stats.ApplyLevelBonus(level);

        Debug.Log($"레벨업! 현재 레벨: {level}, HP: {stats.maxHP}, 공격력: {stats.attackPower}");
    }

    public void TakeDamage(int damage)
    {
        stats.currentHP -= damage;
        Debug.Log($"플레이어 피해! 현재 체력: {stats.currentHP}");

        if (stats.currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("플레이어 사망");
        // GameOver 처리
    }
}
