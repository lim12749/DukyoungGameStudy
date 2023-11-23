using System;
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

        public Transform planet;
        public float gravity = -9.81f;
        public float rotateSpeed;
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
            //m_Rigidbody.MovePosition(m_Rigidbody.position + nextVec);

            transform.Translate(nextVec);
            m_Rigidbody.AddForce((transform.position - planet.position).normalized * gravity);
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.FromToRotation(transform.up, (transform.position - planet.position).normalized) * transform.rotation, rotateSpeed * Time.deltaTime);
        }
        //플레이어 죽음
        void OnCollisionEnter(Collision col)
        {
            if (col.collider.tag == "Meteor")
            {

                Debug.Log("충돌");
               // MainGameManager.instance.isGameOver = true;
               // m_anim.SetBool("Dead", MainGameManager.instance.isGameOver);

               // MainGameManager.instance.EndGame();
                this.enabled = false;

            }
        }
    }
}
