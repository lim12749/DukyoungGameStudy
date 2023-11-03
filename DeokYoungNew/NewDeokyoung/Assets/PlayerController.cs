using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BasketBall
{
    public class PlayerController : MonoBehaviour
    {
        Vector3 direction;
        public float MoveSpeed;//

        private void Update()
        {
            PlayerMove();
            PlayerSpeed();
        }

        public void PlayerMove()
        {
            //이동방향 정의
            direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            transform.position += direction * MoveSpeed * Time.deltaTime ;

            transform.LookAt(transform.position + direction);
        }
        public void PlayerSpeed()
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("누르는중");
            }
        }
    }
}
