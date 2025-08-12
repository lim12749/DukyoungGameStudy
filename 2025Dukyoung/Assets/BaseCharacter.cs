using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public CharacterStats stats;

    protected virtual void Start()
    {
        stats.currentHP = stats.maxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        stats.currentHP -= damage;
        if (stats.currentHP <= 0)
            Die();
    }

    protected abstract void Die();
}
