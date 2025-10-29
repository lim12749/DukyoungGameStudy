using UnityEngine;

public class TowerKillZone : MonoBehaviour
{
      [Tooltip("이 KillZone이 보고할 Tower")]
    public Tower tower;

    [Header("간단 필터")]
    public string enemyTag = "Enemy";        // 태그 이름
    public string enemyLayerName = "Enemy";  // 레이어 이름 (선택)

    int _enemyLayer = -1;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    void Awake()
    {
        if (tower == null) tower = GetComponentInParent<Tower>();
        // 레이어 이름이 유효하면 캐시
        if (!string.IsNullOrEmpty(enemyLayerName))
            _enemyLayer = LayerMask.NameToLayer(enemyLayerName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsEnemy(other.gameObject)) return;

        // 진행도 증가
        if (tower != null) tower.AddProgress(5f); // 필요 시 인스펙터로 빼도 됨

        // 적 삭제(루트 기준)
        var root = other.attachedRigidbody ? other.attachedRigidbody.transform.root : other.transform.root;
        Destroy(root.gameObject);
    }

    bool IsEnemy(GameObject go)
    {
        // 태그 또는 레이어 중 하나만 맞아도 true
        bool tagMatch   = !string.IsNullOrEmpty(enemyTag) && go.CompareTag(enemyTag);
        bool layerMatch = (_enemyLayer >= 0) && (go.layer == _enemyLayer);
        return tagMatch || layerMatch;
    }
}
