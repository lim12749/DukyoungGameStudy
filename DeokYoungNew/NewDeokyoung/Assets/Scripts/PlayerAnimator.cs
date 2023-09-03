using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private Vector3 velcity;
    public bool isWork { get; set; }
    private Animator myAnimator;

    private void Start()
    {
        isWork = false;
        myAnimator = GetComponentInChildren<Animator>();

    }
    private void Update()
    {

        myAnimator.SetBool("IsRun", isWork);

    }

    public void MoveAnimationSet(Vector3 _Velocity)
    {
        velcity = _Velocity;
        isWork = velcity != Vector3.zero;
    }
    public void AttackAnimStart()
    {
        myAnimator.SetTrigger("attack");
    }
}
