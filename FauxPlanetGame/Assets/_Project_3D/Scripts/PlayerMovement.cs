using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed      = 8f;     // 최고 속도 (m/s)
    public float accel         = 30f;    // 가속도  (m/s²)
    public float decelDrag     = 4f;     // 키를 떼었을 때 감속용 드래그

    Rigidbody rb;
    Vector2   input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;     // 지터 최소화
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");  // A(−1) / D(+1)
        input.y = Input.GetAxisRaw("Vertical");    // W(+1) / S(−1)
    }

    void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        /* 1️⃣ 가속 : 원하는 방향으로 힘 가하기 */
        if (moveDir.sqrMagnitude > 0f)
        {
            rb.linearDamping = 0f;   // 이동 중엔 마찰 X

            // 현재 속도가 maxSpeed 이하일 때만 힘 추가
            Vector3 planarVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (planarVel.magnitude < maxSpeed)
                rb.AddForce(moveDir * accel, ForceMode.Acceleration);
        }
        /* 2️⃣ 감속 : 키를 떼면 Drag로 부드럽게 멈춤 */
        else
        {
            rb.linearDamping = decelDrag;
        }
    }
}
