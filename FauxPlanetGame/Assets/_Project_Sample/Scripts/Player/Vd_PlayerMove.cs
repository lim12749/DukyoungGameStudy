using UnityEngine;

public class Vd_PlayerMove : MonoBehaviour
{
    public float speed = 8f;
    Rigidbody rb; Vector2 inVec;
    PlayerStates stats;
    void Awake()
{
    rb = GetComponent<Rigidbody>();
    stats = GetComponent<PlayerStates>();
}
    void Update()
    {                               // ① 입력 벡터
        inVec.x = Input.GetAxisRaw("Horizontal");
        inVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {                               // ② 이동 벡터
        //Vector3 v = new Vector3(inVec.x, 0, inVec.y).normalized * speed;
        Vector3 vel = new Vector3(inVec.x,0,inVec.y).normalized * stats.MoveSpeed;
        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);
    }
}
