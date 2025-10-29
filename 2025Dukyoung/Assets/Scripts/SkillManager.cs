using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public enum Skill { None, Lightning, DragSpawn, Skill3 }

    [Header("Actions (New Input System)")]
    [SerializeField] private InputActionReference pointerPositionAction; // Vector2
    [SerializeField] private InputActionReference pointerPressAction;    // Button

    [Header("References")]
    public Camera mainCam;
    public LayerMask groundLayer;
    public GameObject spherePrefab;

    [Header("Lightning (Skill 1)")]
    public float lightningRadius = 2.5f;
    public LayerMask enemyLayer;

    [Header("Drag Spawn (Skill 2)")]
    public float dragSphereStep = 1.0f;
    public float dragSphereScale = 1.0f;

    [Header("Cooldown (seconds)")]
    public float skill1Cooldown = 3f;
    public float skill2Cooldown = 5f;
    public float skill3Cooldown = 8f;

    [Header("Cooldown UI (각각 0~1로 표시)")]
    [Tooltip("원형 Image(Type=Filled, Method=Radial360)를 쓰면 fillAmount로 표시")]
    public Image  skill1Radial;  // 선택
    public Image  skill2Radial;  // 선택
    public Image  skill3Radial;  // 선택
    [Tooltip("일반 Slider를 쓰고 싶으면 Value(0~1)로 표시")]
    public Slider skill1Slider;  // 선택
    public Slider skill2Slider;  // 선택
    public Slider skill3Slider;  // 선택
    [Tooltip("스킬 버튼(있으면 쿨타임 중 비활성화)")]
    public Button skill1Button;  // 선택
    public Button skill2Button;  // 선택
    public Button skill3Button;  // 선택

    [Header("General")]
    public Skill currentSkill = Skill.None;

    // 내부 상태
    Vector3 _lastSpawnPos;
    bool _isDragging;

    // 쿨타임 진행(0~1). 1이면 사용 가능, 0에서 시작해 쿨타임동안 1까지 채움
    float _cd1 = 1f, _cd2 = 1f, _cd3 = 1f;

    // 에지 플래그
    bool _pressedDown, _releasedUp;

    void Reset()
    {
        if (!mainCam) mainCam = Camera.main;
    }

    void OnEnable()
    {
        if (pointerPositionAction) pointerPositionAction.action.Enable();
        if (pointerPressAction)
        {
            var a = pointerPressAction.action;
            a.Enable();
            a.started  += OnPressStarted;
            a.canceled += OnPressCanceled;
        }
        // 시작 시 UI 동기화
        SyncCooldownUI();
    }

    void OnDisable()
    {
        if (pointerPressAction)
        {
            var a = pointerPressAction.action;
            a.started  -= OnPressStarted;
            a.canceled -= OnPressCanceled;
            a.Disable();
        }
        if (pointerPositionAction) pointerPositionAction.action.Disable();
    }

    public void SelectSkill1() => currentSkill = Skill.Lightning;
    public void SelectSkill2() => currentSkill = Skill.DragSpawn;
    public void SelectSkill3() => currentSkill = Skill.Skill3;

    void OnPressStarted(InputAction.CallbackContext _) { _pressedDown = true; }
    void OnPressCanceled(InputAction.CallbackContext _) { _releasedUp = true; }

    void Update()
    {
        // 1) 쿨타임 진행(0->1)
        TickCooldown(ref _cd1, skill1Cooldown, skill1Radial, skill1Slider, skill1Button);
        TickCooldown(ref _cd2, skill2Cooldown, skill2Radial, skill2Slider, skill2Button);
        TickCooldown(ref _cd3, skill3Cooldown, skill3Radial, skill3Slider, skill3Button);

        // 2) UI 위면 입력 무시
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        { ClearEdge(); return; }

        // 3) 포인터 읽기
        Vector2 screenPos = pointerPositionAction ? pointerPositionAction.action.ReadValue<Vector2>() : Vector2.zero;
        bool isHeld = pointerPressAction && pointerPressAction.action.IsPressed();

        // 4) 스킬 처리
        switch (currentSkill)
        {
            case Skill.Lightning:
                if (_pressedDown && CanCast(Skill.Lightning) && TryGetGroundPoint(screenPos, out var hit))
                {
                    DoLightning(hit);
                    TriggerCooldown(Skill.Lightning);
                }
                break;

            case Skill.DragSpawn:
                // 드래그 시작 시 한 번만 시전 체크 → 쿨타임 시작
                if (_pressedDown && CanCast(Skill.DragSpawn) && TryGetGroundPoint(screenPos, out var start))
                {
                    _isDragging = true;
                    _lastSpawnPos = start;
                    SpawnSphere(start, dragSphereScale);
                    TriggerCooldown(Skill.DragSpawn);
                }
                // 드래그 중에 궤적 구체 생성
                if (_isDragging && isHeld && TryGetGroundPoint(screenPos, out var cur))
                {
                    float dist = Vector3.Distance(_lastSpawnPos, cur);
                    if (dist >= dragSphereStep)
                    {
                        int steps = Mathf.FloorToInt(dist / dragSphereStep);
                        Vector3 dir = (cur - _lastSpawnPos).normalized;
                        for (int i = 1; i <= steps; i++)
                        {
                            var p = _lastSpawnPos + dir * (dragSphereStep * i);
                            SpawnSphere(p, dragSphereScale);
                        }
                        _lastSpawnPos = cur;
                    }
                }
                if (_isDragging && _releasedUp) _isDragging = false;
                break;

            case Skill.Skill3:
                // TODO: 스킬3 정의 시 동일 패턴 적용:
                // if (_pressedDown && CanCast(Skill.Skill3)) { CastSomething(); TriggerCooldown(Skill.Skill3); }
                break;
        }

        ClearEdge();
    }

    // ===== 쿨타임 로직 =====
    bool CanCast(Skill s)
    {
        switch (s)
        {
            case Skill.Lightning: return _cd1 >= 1f || skill1Cooldown <= 0f;
            case Skill.DragSpawn: return _cd2 >= 1f || skill2Cooldown <= 0f;
            case Skill.Skill3:    return _cd3 >= 1f || skill3Cooldown <= 0f;
        }
        return false;
    }

    void TriggerCooldown(Skill s)
    {
        switch (s)
        {
            case Skill.Lightning: _cd1 = (skill1Cooldown <= 0f) ? 1f : 0f; break;
            case Skill.DragSpawn: _cd2 = (skill2Cooldown <= 0f) ? 1f : 0f; break;
            case Skill.Skill3:    _cd3 = (skill3Cooldown <= 0f) ? 1f : 0f; break;
        }
        SyncCooldownUI();
    }

    void TickCooldown(ref float cd, float seconds, Image radial, Slider slider, Button btn)
    {
        if (seconds <= 0f) { cd = 1f; } // 쿨타임 없음
        else if (cd < 1f)
        {
            cd += Time.deltaTime / Mathf.Max(0.0001f, seconds);
            if (cd > 1f) cd = 1f;
        }

        // UI 반영 (0~1)
        if (radial) radial.fillAmount = cd;
        if (slider)
        {
            // 슬라이더를 0~1 게이지로 쓰는 경우:
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.SetValueWithoutNotify(cd);
        }
        if (btn) btn.interactable = (cd >= 1f);
    }

    void SyncCooldownUI()
    {
        if (skill1Radial) skill1Radial.fillAmount = _cd1;
        if (skill2Radial) skill2Radial.fillAmount = _cd2;
        if (skill3Radial) skill3Radial.fillAmount = _cd3;

        if (skill1Slider) { skill1Slider.minValue = 0; skill1Slider.maxValue = 1; skill1Slider.SetValueWithoutNotify(_cd1); }
        if (skill2Slider) { skill2Slider.minValue = 0; skill2Slider.maxValue = 1; skill2Slider.SetValueWithoutNotify(_cd2); }
        if (skill3Slider) { skill3Slider.minValue = 0; skill3Slider.maxValue = 1; skill3Slider.SetValueWithoutNotify(_cd3); }

        if (skill1Button) skill1Button.interactable = (_cd1 >= 1f);
        if (skill2Button) skill2Button.interactable = (_cd2 >= 1f);
        if (skill3Button) skill3Button.interactable = (_cd3 >= 1f);
    }

    // ===== 공통 유틸 =====
    bool TryGetGroundPoint(Vector2 screenPos, out Vector3 hitPoint)
    {
        var ray = mainCam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit, 500f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            hitPoint = hit.point;
            return true;
        }
        hitPoint = default;
        return false;
    }

    void DoLightning(Vector3 center)
    {
        var vis = SpawnSphere(center + Vector3.up * 0.1f, lightningRadius * 2f);

        var hits = Physics.OverlapSphere(center, lightningRadius, enemyLayer, QueryTriggerInteraction.Ignore);
        foreach (var h in hits)
        {
            var enemy = h.GetComponent<IEnemy>();
            if (enemy != null) enemy.Die();
            else Destroy(h.gameObject);
        }

        Destroy(vis, 0.5f);
    }

    GameObject SpawnSphere(Vector3 pos, float scale)
    {
        GameObject go = spherePrefab
            ? Instantiate(spherePrefab, pos, Quaternion.identity)
            : GameObject.CreatePrimitive(PrimitiveType.Sphere);

        if (!spherePrefab)
        {
            var col = go.GetComponent<Collider>();
            if (col) Destroy(col);
        }

        go.transform.position = pos;
        go.transform.localScale = Vector3.one * scale;
        return go;
    }

    void ClearEdge() { _pressedDown = _releasedUp = false; }
}
