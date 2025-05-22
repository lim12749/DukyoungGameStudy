using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool IsAiming => Input.GetMouseButton(1);
    public bool IsFiring => Input.GetButton("Fire1");

    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    void Update()
    {
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        JumpPressed = Input.GetButtonDown("Jump");

        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
    }
    
}
