using UnityEngine;

/// <summary>
/// 아이템 정의를 위한 ScriptableObject 클래스
/// 인벤토리 시스템에서 사용되는 아이템의 기본 정보를 저장합니다.
/// </summary>
[CreateAssetMenu(menuName = "Demo/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [Header("아이템 기본 정보")]
    [Tooltip("아이템의 고유 식별자 (중복되지 않는 문자열)")]
    public string id;
    
    [Tooltip("게임 내에서 표시될 아이템 이름")]
    public string displayName;
    
    [Tooltip("아이템의 아이콘 이미지")]
    public Sprite icon;
}
