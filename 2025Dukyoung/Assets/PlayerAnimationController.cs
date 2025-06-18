using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;

    private readonly int SpeedHash = Animator.StringToHash("Speed");
    //private readonly int IsJumpingHash = Animator.StringToHash("isJumping");

    void Awake()
    {
       // animator = GetComponent<Animator>();
    }
        /// <summary>
    /// 이동 속도 값을 기반으로 애니메이션 Blend를 조절합니다.
    /// </summary>
    /// <param name="speed">현재 이동 속도 (0 ~ 1)</param>
    public void UpdateMoveAnimation(float speed)
    {
        animator.SetFloat(SpeedHash, speed);
    }

    /// <summary>
    /// 점프 애니메이션 여부를 설정합니다.
    /// </summary>
    /// <param name="isJumping">점프 중인지 여부</param>
    public void SetJumping(bool isJumping)
    {
        Debug.Log(isJumping);
        animator.SetBool("isJumping", isJumping);

    }
}
