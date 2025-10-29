using UnityEngine;

public class RightClickSkillSlot : MonoBehaviour
{
    [Header("Default Equip")]
    [SerializeField] private GameObject defaultSkillPrefab; // 시작 시 기본 장착할 스킬 프리팹

    [Header("Runtime")]
    public Transform skillParent; // 비우면 this.transform
    MonoBehaviour currentComp;    // 장착된 컴포넌트(디스플레이용)
    IRightClickSkill current;     // 실행 인터페이스

    void Awake()
    {
        if (!skillParent) skillParent = transform;
    }

    void Start()
    {
        // 시작 시 기본 스킬 자동 장착
        if (defaultSkillPrefab) Equip(defaultSkillPrefab);
        else Debug.LogWarning("[RightClickSkillSlot] 기본 스킬 프리팹이 비어있습니다.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) current?.TryCast(); // 우클릭 발동
    }

    // 외부(픽업 등)에서 호출: 프리팹 교체 장착
    public void Equip(GameObject skillPrefab)
    {
        if (!skillPrefab) return;

        if (currentComp) Destroy(currentComp.gameObject);

        var go = Instantiate(skillPrefab, skillParent);
        currentComp = go.GetComponent<MonoBehaviour>();
        current     = go.GetComponent<IRightClickSkill>();
        if (current == null)
        {
            Debug.LogError("[RightClickSkillSlot] 장착 프리팹에 IRightClickSkill가 없습니다.");
            Destroy(go);
            currentComp = null;
            return;
        }

        var stats = GetComponent<PlayerStats>();
        current.Initialize(gameObject, stats);
    }
}
