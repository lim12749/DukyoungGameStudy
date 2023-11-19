using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor{
    public class Enemy : MonoBehaviour
    {
        public float speed;
        public float Health;
        public float maxHealth;
        public RuntimeAnimatorController[] animcon;
        public Animator anim;
        public Rigidbody2D target;

        bool isLive;

        Rigidbody2D m_rigid;
        SpriteRenderer m_sprites;
        // Start is called before the first frame update
        void Start()
        {
            m_rigid = GetComponent<Rigidbody2D>();
            m_sprites = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            target = GameManager.instace.player.GetComponent<Rigidbody2D>();

        }
        private void OnEnable()
        {
            target = GameManager.instace.player.GetComponent<Rigidbody2D>();
            isLive = true;
            Health = maxHealth;
        }
        private void FixedUpdate()
        {
            Locomotion();
        }
        private void LateUpdate()
        {
            if (!isLive)
                return;

            FlipSprite();
        }
        private void FlipSprite()
        {
            //목표 x축 값과 자신의 x축 값을 비교하여 작으면 true가 되도록함
            m_sprites.flipX = target.position.x < m_rigid.position.x;
        }
        private void Locomotion()
        {
            Vector2 dirVec = target.position - m_rigid.position;
            Vector2 nectVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            m_rigid.MovePosition(m_rigid.position + nectVec);
            m_rigid.velocity = Vector2.zero; //물리속도가 이동에 영양을 주지않게함.
        }
        public void Init(SpwanData spwanData)
        {
            anim.runtimeAnimatorController = animcon[spwanData.SpriteType];
            speed = spwanData.Speed;
            maxHealth = spwanData.Health;
            Health = spwanData.Health;
        }
    }
}
