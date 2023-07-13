using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Like{
    public class Player : MonoBehaviour
    {
        public float Speed;
        private Rigidbody myRigidbody;

        [SerializeField] private Vector3 inputVec;

        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            Vector3 dir = new Vector3(inputVec.x, 0f, inputVec.y);
            Vector3 nextVec = dir * Speed * Time.deltaTime; //
            
            myRigidbody.MovePosition(myRigidbody.position + nextVec);
        }

        //inputsystem?? ?? ????? value
         void OnMove(InputValue _inputValue)
        {
            var value = _inputValue.Get<Vector2>();
            inputVec = value;

        }
    }
}
