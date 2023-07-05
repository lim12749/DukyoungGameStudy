using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Like {
    public class PlayerInputManager : MonoBehaviour
    {
        public Vector3 InputVector { get; private set; }

        private void Update()
        {
            MoveInput();
        }

        private void MoveInput()
        {
            InputVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        }
    }
}
