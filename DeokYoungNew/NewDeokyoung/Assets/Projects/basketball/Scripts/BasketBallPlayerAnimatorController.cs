using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasketBallGame
{
    public class BasketBallPlayerAnimatorController : MonoBehaviour
    {
        private Vector3 velcity;
        public bool isRun { get; set; }
        private Animator myAnimator;

        private void Start()
        {
            isRun = false;
            myAnimator = GetComponentInChildren<Animator>();
        }
        private void Update()
        {
            myAnimator.SetBool("IsRun", isRun);
        }
        public void MoveAnimationSet(Vector3 _Velocity)
        {
            velcity = _Velocity;
            isRun = velcity != Vector3.zero;
        }
    }
}
