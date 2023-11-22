using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Survivor{ 
    public class Player : MonoBehaviour
    {
        public float moveSpeed;

        public Vector2 inputVec;
        private Rigidbody2D m_Rigidbody2d;
        private SpriteRenderer m_spriter;
        private Animator m_anim;
        private void Awake()
        {
            m_spriter = GetComponent<SpriteRenderer>();
            m_Rigidbody2d = GetComponent<Rigidbody2D>();
            m_anim = GetComponent<Animator>();
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
            Vector2 nextVec = inputVec * moveSpeed * Time.deltaTime;
            m_Rigidbody2d.MovePosition(m_Rigidbody2d.position + nextVec);
        }
    }
}
