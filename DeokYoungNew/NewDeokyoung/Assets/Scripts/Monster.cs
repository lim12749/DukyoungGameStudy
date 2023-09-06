using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Monster : LivingEntity
{
    public int hp;
    public Slider hpUI;
    private UnityEngine.AI.NavMeshAgent pathFinder; // ��ΰ�� AI ������Ʈ
    public GameObject SelectImage;
    private void Start()
    {
        hpUI.maxValue = hp;
    }
    public void SetDamage(int _value)
    {
        hp = hp - _value;
        hpUI.value = hp;
        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("����");
        Destroy(this.gameObject);
    }
}
