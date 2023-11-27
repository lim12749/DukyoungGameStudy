using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKeyInput : MonoBehaviour
{
    public Vector2 InputVector { get; set; }
    public Vector2 mousePosition { get; set; }
    public void OnMove(InputValue inputValue)
    {
        InputVector = inputValue.Get<Vector2>();
    }

    public void OnLook(InputValue inputValue)
    {
        mousePosition = inputValue.Get<Vector2>();
    }

}
