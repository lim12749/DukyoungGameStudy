using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace BasketBallGame
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController m_characterController;
        private Camera m_MainCamera;

        [SerializeField] private Vector3 direction;
        private Vector3 m_viewRight;
        private Vector3 m_viewForward;

        private float gravity = -9.81f;

        public float MoveSpeed;//

        private void Awake()
        {
            m_characterController = GetComponent<CharacterController>();
            m_MainCamera = Camera.main.GetComponent<Camera>();

            SetCameraViewDirection();
        }

        private void Update()
        {
            PlayerMove();
            PlayerSpeed();
        }

        private void SetCameraViewDirection()
        {
            m_viewForward = m_MainCamera.transform.forward;
            m_viewForward.y = 0;
            m_viewForward  = Vector3.Normalize(m_viewForward);

            m_viewRight = Quaternion.Euler(new Vector3(0, 90, 0)) * m_viewForward;


        }
        public void PlayerMove()
        {
            Vector3 _playerRight = m_viewRight * Time.deltaTime * Input.GetAxis("Horizontal");
            Vector3 _playerForward = m_viewForward * Time.deltaTime * Input.GetAxis("Vertical");
            direction = _playerForward + _playerRight;

        
            if (direction!= Vector3.zero)
            {
               
                transform.forward = direction;

                m_characterController.Move(direction * MoveSpeed);
            }
        }
        public void PlayerSpeed()
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("????????");
            }
        }
    }
}
