using UnityEngine;

/// <summary>
/// 마우스로 Y축 회전, 키보드로 이동 (TPS 전형 구조)
/// </summary>
[RequireComponent(typeof(CharacterController))]
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

    /*
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float rotateSpeed = 120f;  // 마우스 회전 속도
    public Transform cameraTransform;

    private CharacterController controller;
    private InputReader inputReader;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputReader = GetComponent<InputReader>();
    }

    void Update()
    {
        Vector2 moveInput = inputReader.MoveInput;

        // 1. 카메라 기준으로 이동 방향 설정 (Y축 제외)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        // 2. 중력 적용
        if (controller.isGrounded)
            verticalVelocity = -1f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        moveDir.y = verticalVelocity;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // 3. 마우스 좌우 회전만 적용 (Y축 회전)
        float mouseX = inputReader.MouseX;
        if (Mathf.Abs(mouseX) > 0.01f)
        {
            transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime);
        }
    }*/
}