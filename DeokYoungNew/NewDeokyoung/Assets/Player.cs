using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MainGame
{
    public class Player : MonoBehaviour
    {
        public float moveSpeed;

        public Vector2 inputVec;
        private Rigidbody m_Rigidbody;
        private SpriteRenderer m_spriter;
        private Animator m_anim;
        private void Awake()
        {
            m_spriter = transform.GetComponentInChildren<SpriteRenderer>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_anim = transform.GetComponentInChildren<Animator>();
        }
        private void FixedUpdate()
        {
            PlayerMove();
        }
        private void LateUpdate()
        {
            //magnitude vector lange
            m_anim.SetFloat("Speed", inputVec.magnitude);
            if (inputVec.x != 0)
            {
                m_spriter.flipX = inputVec.x < 0; 
            }
        }
        private void OnMove(InputValue _value)
        {
            inputVec = _value.Get<Vector2>();
        }
        private void PlayerMove()
        {
            Vector3 nextVec =  new Vector3(inputVec.x, 0f, inputVec.y) * moveSpeed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + nextVec);
        }
    }
}
