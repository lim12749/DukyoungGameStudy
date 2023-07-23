using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tree
{
    public class CameraController : MonoBehaviour
    {
        InputKey inputKey;
        Transform cameraTransform;
        private float xRotate, yRotate, xRotateMove, yRotateMove;
        public float rotateSpeed = 500.0f;

        void Init()
        {
            cameraTransform = Camera.main.transform;
            inputKey = GameObject.FindObjectOfType<InputKey>();
        }
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            //move
            var moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            transform.Translate(moveDir*Time.deltaTime*10f);
            // click To Rotate
            if(Input.GetMouseButton(1))
            {
                var xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed; ;
                var yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed; ;

                yRotate = yRotate + yRotateMove;
                //xRotate = transform.eulerAngles.x + xRotateMove; 
                xRotate = xRotate + xRotateMove;

                xRotate = Mathf.Clamp(xRotate, -90, 90); // 위, 아래 고정

                //transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

                Quaternion quat = Quaternion.Euler(new Vector3(xRotate, yRotate, 0));
                transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime /* x speed */);
            }

        }
    }
}
