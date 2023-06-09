using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tree
{
    public class InputKey : MonoBehaviour
    {
        Camera ViewCamera;
        public Vector3 MousePostion { get; private set; }

        private void Start()
        {
            ViewCamera = Camera.main;
        }
        private void Update()
        {
            MousePositoin();
        }
        public void MousePositoin()
        {
            MousePostion = Input.mousePosition;
            //Debug.Log($"x: {MousePostion.x}, y: {MousePostion.y}");
            
        }
    }
}
