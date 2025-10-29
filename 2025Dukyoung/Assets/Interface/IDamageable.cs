using UnityEngine;

/// IDamageable 인터페이스: 데미지를 받을 수 있는 객체에 구현
public interface IDamageable 
{
    void TakeDamage(float amount);
}