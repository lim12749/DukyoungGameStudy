using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("HP")]
    public float maxHP = 30f;
    [SerializeField] float currentHP;

    [Header("On Death")]
    public int expOnDeath = 3;        // 죽을 때 줄 경험치

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"[Enemy] Took {amount} damage.");
        if (currentHP <= 0f) return;
        currentHP = Mathf.Max(0f, currentHP - amount);

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerExperience.Instance?.AddExp(expOnDeath);
        Destroy(gameObject);
    }
}
