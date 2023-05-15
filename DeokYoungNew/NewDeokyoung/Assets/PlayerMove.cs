using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputAction moveAction;
    private CharacterController controller;
    public float playerSpeed = 1.0f;

    Vector3 moveDirection;
    Vector3 dir;

    private void Awake()
    {

        moveAction = playerInput.actions["Move"];
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 direction = new Vector3(input.x, 0f, input.y);


        if (direction != Vector3.zero)
        {
            //방향을 바라보게합니다
            transform.forward = direction;
            //transform.position += FinalMovement.normalized * moveSpeed * Time.deltaTime;
            controller.Move(direction * playerSpeed*Time.deltaTime);
        }
    }
}
