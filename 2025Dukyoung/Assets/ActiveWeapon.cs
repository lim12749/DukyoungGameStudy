using UnityEngine;
using UnityEngine.Animations.Rigging;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

public class ActiveWeapon : MonoBehaviour
{
    private ProjectileWeapon currentWeapon; // 현재 장착된 무기
    public Rig handIKRig;                   // 손 IK 레이어 
    public Transform weaponHolder;          // 무기 장착 위치
    public Transform weaponLeftGrip;        // 왼손 그립
    public Transform weaponRightGrip;       // 오른손 그립

    // 애니메이션
    private Animator animator;
    private AnimatorOverrideController aoc;

    // 베이스 컨트롤러에서 비워둔 자리(placeholder) 클립의 키(이름)
    // ※ 반드시 베이스 컨트롤러에 있는 '원본 클립' 이름과 동일해야 함
    [SerializeField] private string baseSlotClipName = "Weapon_anim_empty";

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // 1) AOC를 확실하게 생성하고 다시 할당
        var baseController = animator.runtimeAnimatorController;
        if (baseController == null)
        {
            Debug.LogError("ActiveWeapon: Animator에 RuntimeAnimatorController가 없습니다.");
            return;
        }

        // 기존이 AOC든 아니든, 새 AOC를 만들어 씌우는 게 가장 안전
        aoc = new AnimatorOverrideController(baseController);
        animator.runtimeAnimatorController = aoc;

        // 시작 시 무기가 있으면 장착 처리
        ProjectileWeapon existingWeapon = GetComponentInChildren<ProjectileWeapon>();
        if (existingWeapon != null) EquipWeapon(existingWeapon);
        else Debug.LogWarning("ActiveWeapon: No ProjectileWeapon found in children.");
    }

    void Update()
    {
        if (currentWeapon != null)
        {
            // 현재 타입이 ProjectileWeapon이면 항상 1 (지금 구조상 true)
            handIKRig.weight = 1f;
            // 레이어 1이 존재하는지 확인 후 가중치 적용 (없으면 무시)
            if (animator.layerCount > 1) animator.SetLayerWeight(1, 1f);
        }
        else
        {
            handIKRig.weight = 0f;
            if (animator.layerCount > 1) animator.SetLayerWeight(1, 0f);
        }
    }

    public void EquipWeapon(ProjectileWeapon weapon)
    {
        // 기존 무기 제거
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = weapon;

        // 홀더에 부착
        weapon.transform.SetParent(weaponHolder, worldPositionStays: false);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        // 레이어 가중치
        if (animator.layerCount > 1) animator.SetLayerWeight(1, 1f);

        // 아주 짧은 지연보다, 바로 오버라이드 + Rebind가 안정적
        ApplyWeaponPoseClip();
    }

    private void ApplyWeaponPoseClip()
    {
        if (currentWeapon == null || currentWeapon.weponAnimationClip == null)
        {
            Debug.LogWarning("ActiveWeapon: 무기 또는 무기 포즈 클립이 없습니다.");
            return;
        }
        if (aoc == null)
        {
            Debug.LogError("ActiveWeapon: AnimatorOverrideController가 초기화되지 않았습니다.");
            return;
        }

        // 2) 키 이름이 실제 베이스 클립과 일치해야 함
        try
        {
            aoc[baseSlotClipName] = currentWeapon.weponAnimationClip;
        }
        catch
        {
            Debug.LogError($"ActiveWeapon: 오버라이드 실패 - '{baseSlotClipName}' 키를 찾을 수 없습니다. " +
                           "베이스 컨트롤러에서 해당 이름의 원본 클립이 상태에 배치되어 있는지 확인하세요.");
            return;
        }

        // 3) 즉시 반영
        animator.Rebind();
        animator.Update(0f);

        // 4) 필요 시 상태 재생 (베이스 상태명이 다르면 수정)
        // animator.Play("WeaponPoseState", 1, 0f); // 예: 레이어1의 포즈 상태
    }

#if UNITY_EDITOR
    [ContextMenu("Save Weapon Position")]
    public void SaveWeapOnPosition()
    {
        // Recorder의 루트는 '포즈를 적용하려는 애니메이터 루트'여야 하며,
        // 바인딩 대상(weaponHolder/그립들)은 반드시 이 루트의 하위여야 곡선이 기록됩니다.
        var rootGO = animator != null ? animator.gameObject : gameObject;

        var recorder = new GameObjectRecorder(rootGO);

        // 경로가 루트 하위인지 검증
        bool IsUnderRoot(Transform t)
            => t != null && t == rootGO.transform || t.IsChildOf(rootGO.transform);

        if (!IsUnderRoot(weaponHolder) || !IsUnderRoot(weaponLeftGrip) || !IsUnderRoot(weaponRightGrip))
        {
            Debug.LogError("ActiveWeapon: Recorder 루트 아래에 없는 트랜스폼이 있습니다. " +
                           "weaponHolder/그립 오브젝트를 Animator 루트 하위로 이동하세요.");
            return;
        }

        recorder.BindComponentsOfType<Transform>(weaponHolder.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);

        // 스냅샷 & 저장
        recorder.TakeSnapshot(Time.time);
        if (currentWeapon == null || currentWeapon.weponAnimationClip == null)
        {
            Debug.LogError("ActiveWeapon: 저장할 대상 클립이 없습니다.");
            return;
        }

        recorder.SaveToClip(currentWeapon.weponAnimationClip);
        Debug.Log("ActiveWeapon: 현재 포즈를 무기 포즈 클립에 저장 완료.");
    }
#endif
}
