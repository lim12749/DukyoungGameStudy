using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerController : MonoBehaviour
{
    [SerializeField] Vector3 viewForward;
    [SerializeField] Vector3 viewRight;
    [SerializeField] Vector3 direction;

    private CharacterController myCC;
    private PlayerAnimator playerAnim;
    public float moveSpeed;
    
    private void Start()
    {
        viewForward = Camera.main.transform.forward;
        viewForward.y = 0; //y성분을 지우기
        viewForward = Vector3.Normalize(viewForward); //정규화

        viewRight = Quaternion.Euler(new Vector3(0, 90, 0)) * viewForward;

        myCC = GetComponent<CharacterController>();
        playerAnim = GetComponent<PlayerAnimator>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }
    public void PlayerMove()
    {
        Vector3 rightMove = viewRight * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 forwardMove = viewForward * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 FinalMoveMent = forwardMove + rightMove;

        direction = Vector3.Normalize(FinalMoveMent);

        playerAnim.MoveAnimationSet(direction);
        if(direction != Vector3.zero)
        {
            transform.forward = direction;//방향으로 돌림

            myCC.Move(direction * moveSpeed);
        }
    }
}
