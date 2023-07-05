using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Like{
    public class Player : MonoBehaviour
    {
        public float Speed;
        private Rigidbody myRigidbody;
        private PlayerInputManager inputManager;

        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
            inputManager = GetComponent<PlayerInputManager>();
        }
        private void FixedUpdate()
        {
            UpdateMove();
        }
        private void UpdateMove()
        {
            //함을준다
            //myRigidbody.AddForce(inputManager.InputVector);

            //속도 제어
            //myRigidbody.velocity = inputManager.InputVector;

            //위치 이동   vector 1로만들고 속도 물리 업데이트에서만 사용하는 델타타임
            Vector3 nomal = inputManager.InputVector.normalized * Speed *Time.fixedDeltaTime; 
            myRigidbody.MovePosition(myRigidbody.position+ nomal);

        }
    }
}
