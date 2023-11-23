using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string GetHorizontal, GetVertical;
    [SerializeField] private float movementSpeed;
    private CharacterController characterController;

   
    public AnimationCurve jumpFalloff;
    [SerializeField] private float jumpMultiplier;
    public KeyCode jumpkey;
    private bool isJumping;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
        JumpInput();
    }
    void PlayerMovement()
    {
        float Verinput = Input.GetAxis(GetVertical) * movementSpeed * Time.deltaTime;
        float Horinput = Input.GetAxis(GetHorizontal) * movementSpeed * Time.deltaTime;

        Vector3 fowrardMovement = transform.forward * Verinput;
        Vector3 rightmovement = transform.right * Horinput;

        characterController.SimpleMove(fowrardMovement + rightmovement);
    }
    void JumpInput()
    {
        if(Input.GetKeyDown(jumpkey)&&!isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }
    private IEnumerator JumpEvent()
    {
        float timeInair = 0.0f;

        do
        {
            float jumpForce = jumpFalloff.Evaluate(timeInair);
            characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInair += Time.deltaTime;//체공시간 누적
            yield return null;
        }
        while (!characterController.isGrounded && characterController.collisionFlags != CollisionFlags.Above);//땅인지 체크

        isJumping = false;
    }
}
