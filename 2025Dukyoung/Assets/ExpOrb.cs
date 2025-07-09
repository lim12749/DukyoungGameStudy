using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expAmount = 10;
    public float moveSpeed = 5f;

    private Transform target;

    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
    }

    void Update()
    {
        if (target == null) return;

        // 플레이어 방향으로 이동
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // 충돌 처리
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            Destroy(gameObject);
            //var expReceiver = target.GetComponent<ExpReceiver>();
            //if (expReceiver != null)
            //{
              //  expReceiver.AddExp(expAmount);
               // Destroy(gameObject);
           // }
        }
    }
}
