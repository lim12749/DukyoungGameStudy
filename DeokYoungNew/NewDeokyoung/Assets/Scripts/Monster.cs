using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : LivingEntity
{
    public float DieTime;
    public int hp;
    public Animator MonsterAnim;
    public Rigidbody MonsterRigidbody;
    public CapsuleCollider MonsterCollider;
    //public Slider hpUI;
    public UnityEngine.AI.NavMeshAgent pathFinder;
    public GameObject Target;
    public bool isMove = false; 

    private void Start()
    {
        //hpUI.maxValue = hp;
    }
    private void Update()
    {
        pathFinder.SetDestination(Target.transform.position);
        MonsterMoveAnim();
    }
    public void MonsterMoveAnim()
    {
        isMove = pathFinder.velocity != Vector3.zero;

        MonsterAnim.SetBool("IsMove", isMove);
    }
    public void SetDamage(int _value)
    {
        hp = hp - _value;
        //hpUI.value = hp;
        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        //is Die play Animator
        MonsterAnim.SetTrigger("IsDie");
        //is Die Off Rigidbody gravity
        MonsterRigidbody.useGravity = false;
        //is Die Off Collider
        MonsterCollider.enabled = false;
        //is Die Off NevMeshAgent
        pathFinder.Stop();

        Destroy(this.gameObject, DieTime);
    }
}
