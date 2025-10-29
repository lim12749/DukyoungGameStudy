using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float maxHP = 100f;
    public float attackDamage = 10f;
    public float abilityPower = 0f;
    public float attackSpeed = 1f;        // 초당 공격 횟수 기준
    [Range(0f,1f)] public float critChance = 0.1f;
    public float moveSpeed = 6f;
    public float armor = 0f;
    [Range(0f,0.6f)] public float cooldownReduction = 0f; // 0~0.6 (60%)

    [Header("Runtime (ReadOnly)")]
    [SerializeField] private float currentHP;

    public float CurrentHP => currentHP;

    void Awake()
    {
        currentHP = Mathf.Clamp(currentHP <= 0 ? maxHP : currentHP, 1f, maxHP);
    }

    public void TakeDamage(float raw)
    {
        float reduced = Mathf.Max(1f, raw - armor);
        currentHP = Mathf.Max(0f, currentHP - reduced);
        if (currentHP <= 0f) Die();
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }

    void Die()
    {
        // TODO: 사망 처리(리스폰/게임오버 등)
        Debug.Log("[Player] Dead");
    }
}
