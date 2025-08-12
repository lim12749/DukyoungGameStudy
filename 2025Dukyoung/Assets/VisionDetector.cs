using UnityEngine;

public class VisionDetector : MonoBehaviour
{
    private BaseEnemy enemy;

    private void Awake()
    {
        // 부모 오브젝트에서 BaseEnemy 참조
        enemy = GetComponentInParent<BaseEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemy == null) return;

        // 태그로 비교 (또는 컴포넌트로 확인해도 됨)
        if (other.CompareTag("Player"))
        {
            Transform target = other.transform;
            enemy.SetTarget(target);
        }
    }
}
