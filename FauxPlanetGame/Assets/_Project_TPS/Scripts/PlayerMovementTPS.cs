using UnityEngine;

/// <summary>
/// 키보드 wasd 입력을 받아 평면 x-z축으로 이동시키는 스크립트
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementTPS : MonoBehaviour
{
     /* ---------- Inspector 노출 ---------- */
    [Header("References")]
    public Transform mainCameraTransform;            // MainCamera

    [Header("Movement Settings")]
    [Tooltip("초당 이동 속도 (m/s)")]
    public float movementSpeedMetersPerSecond = 8f;

    /* ---------- 내부 변수 ---------- */
    private Rigidbody playerRigidbody;               // 물리 컴포넌트
    private Vector2   keyboardInput;                 // (x = 좌/우, y = 앞/뒤)

    /* ---------- Unity 생명 주기 ---------- */
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()        // 매 프레임: 입력만 처리
    {
        keyboardInput.x = Input.GetAxisRaw("Horizontal"); // A/D
        keyboardInput.y = Input.GetAxisRaw("Vertical");   // W/S
    }

    private void FixedUpdate()   // 물리 갱신 주기
    {
        MoveWithCameraBasis();
    }

    /* ---------- 핵심 이동 로직 ---------- */
    private void MoveWithCameraBasis()
    {
        /* 1) 카메라의 ‘앞·옆’ 벡터를 평면으로 투영해 얻는다 */
        Vector3 cameraForward = Vector3.ProjectOnPlane(
                                   mainCameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight   = Vector3.ProjectOnPlane(
                                   mainCameraTransform.right,  Vector3.up).normalized;

        /* 2) 입력을 카메라 축에 곱해 ‘원하는 이동 방향’ 계산 */
        Vector3 desiredDirection =
            (cameraRight   * keyboardInput.x +
             cameraForward * keyboardInput.y).normalized;      // ← y → z 로 수정!

        /* 3) 목표 속도 = 방향 × 설정 속도 */
        Vector3 desiredVelocity = desiredDirection * movementSpeedMetersPerSecond;

        /* 4) Y(중력) 성분은 유지하고, XZ 속도만 교체 */
        Vector3 currentVertical =
            Vector3.up * playerRigidbody.linearVelocity.y;

        playerRigidbody.linearVelocity = desiredVelocity + currentVertical;  // ← velocity 로 교체

        /* 5) 이동 중이면 몸을 그 방향으로 회전 */
        if (desiredDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(desiredDirection, Vector3.up);

            playerRigidbody.MoveRotation(Quaternion.Slerp(
                playerRigidbody.rotation,
                targetRotation,
                15f * Time.fixedDeltaTime));
        }
    }

    /*
    void planMove(){
        // 입력값 한곳에 저장
        Vector3 movementDirection = new Vector3(KeyboardInputVector.x,0f,KeyboardInputVector.z);
        //방향백터를 normalized로 길이 1인 단위백터로 만든다. 
        //-> 길이가 1이면 곱하기로 속도 조정이 간단해진다.
        Vector3 unitDirection = movementDirection.normalized; 
        //최종 목표 속도 = 방향(길이1) * 이동속도(설정 값)
        Vector3 targetVelocity = unitDirection *movementSpeedMetersPerSecond;
            /* 4) Rigidbody.velocity 로 실제 속도 적용
              단, y 축(중력 방향) 속도는 기존 값을 그대로 보존. 
        Vector3 currentVerticalVelocity = Vector3.up * playerRigidbody.linearVelocity.y;

        //이동 적용 
        playerRigidbody.linearVelocity = targetVelocity + currentVerticalVelocity;
    }*/
}
