using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    private float verticalVelocity;
    private CharacterController controller;
    private PlayerInputReader input;
    public Transform cameraTransform;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputReader>();
    }

    void Update()
    {
        Vector2 move = input.MoveInput;
        Vector3 moveDir = cameraTransform.right * move.x + cameraTransform.forward * move.y;
        moveDir.y = 0f;

        if (controller.isGrounded)
            verticalVelocity = -1f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        moveDir.y = verticalVelocity;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }
}
