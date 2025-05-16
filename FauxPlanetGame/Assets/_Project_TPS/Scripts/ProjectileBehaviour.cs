using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
 [Tooltip("총알이 날아가는 기본 속도 (m/s)")]
    public float launchSpeedMetersPerSecond = 25f;

    [Tooltip("몇 초 뒤 총알을 자동으로 제거할지")]
    public float lifeTimeSeconds = 2f;

    private void Start()
    {
        /* Rigidbody 가져오기 */
        Rigidbody projectileRigidbody = GetComponent<Rigidbody>();

        /* ‘앞 방향’(local Z+) 으로 순간 힘(Impulse) 가하기 */
        projectileRigidbody.AddForce(
            transform.forward * launchSpeedMetersPerSecond,
            ForceMode.Impulse);

        /* lifeTimeSeconds 후 자기 파괴 */
        Destroy(gameObject, lifeTimeSeconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        /* 플레이어와 부딪히면 무시, 그 외에는 삭제 */
        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
