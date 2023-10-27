using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity
{

    public Slider healthSlider; 
    public Animator animator;
    public PLayerController playerMovecontroller;
    public FindTargetFOV playerFOV;
    public PlayerAnimator playerAnimator;

    protected override void OnEnable()
    {
        base.OnEnable();    

        healthSlider.gameObject.SetActive(true); 

        healthSlider.maxValue = StartingHealth; 

        healthSlider.value = Health; 

        playerMovecontroller.enabled = true;
        playerFOV.enabled = true;

        Dead = false;
    }

  
    public override void OnDamage(float _damage)
    {
        if(!Dead) 
        {

        }
        base.OnDamage(_damage);

        healthSlider.value = Health;
    }
    public override void Die()
    {
        base.Die(); 

        healthSlider.gameObject.SetActive(false);

        animator.SetTrigger("death");

        playerMovecontroller.enabled = false;
        playerFOV.enabled = false;
        //playerAnimator.enabled = false;


    }
    public void OnTriggerEnter(Collider other)
    {
       
    }
}
