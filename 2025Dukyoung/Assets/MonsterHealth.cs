// MonsterHealth.cs
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class MonsterHealth : MonoBehaviour
{
    [SerializeField] float maxHP = 50f;
    [SerializeField] GameObject deathFx; // 선택
    public float Current { get; private set; }

    public event Action OnDeath;

    void Awake() => Current = maxHP;

    public void TakeDamage(float dmg)
    {
        if (Current <= 0f) return;
        Current -= dmg;
        if (Current <= 0f)
        {
            OnDeath?.Invoke();
            if (deathFx) Instantiate(deathFx, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
