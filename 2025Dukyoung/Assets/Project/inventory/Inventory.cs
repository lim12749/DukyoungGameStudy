using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 인벤토리를 관리하는 클래스
/// 아이템의 추가, 제거, 순서 변경 등의 기능을 제공합니다.
/// </summary>
public class Inventory : MonoBehaviour
{
    /// <summary>
    /// 인벤토리 슬롯에 들어갈 아이템 엔트리
    /// Inspector에서 직렬화 가능하도록 Serializable로 선언
    /// </summary>
    [Serializable] 
    public class Entry 
    { 
        [Tooltip("이 슬롯에 들어있는 아이템 정의")]
        public ItemDefinition def; 
    }

    [Header("인벤토리 데이터")]
    [Tooltip("현재 보유 중인 아이템들의 리스트")]
    public List<Entry> items = new List<Entry>();
    
    
    [Tooltip("인벤토리가 변경될 때 호출되는 이벤트 (UI 업데이트용)")]
    public event Action OnChanged;

    /// <summary>
    /// 인벤토리에 새로운 아이템을 추가합니다.
    /// </summary>
    /// <param name="def">추가할 아이템 정의</param>
    public void Add(ItemDefinition def)
    {
        if (def == null) return;
        items.Add(new Entry { def = def });   // ★ 뒤에 추가 = 획득 순서 유지
        OnChanged?.Invoke(); // UI 업데이트 알림
    }

    /// <summary>
    /// 지정된 인덱스의 아이템을 인벤토리에서 제거합니다.
    /// </summary>
    /// <param name="index">제거할 아이템의 인덱스</param>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= items.Count) return;
        items.RemoveAt(index);
        OnChanged?.Invoke(); // UI 업데이트 알림
    }

    /// <summary>
    /// 두 아이템의 위치를 서로 바꿉니다 (드래그 앤 드롭용).
    /// </summary>
    /// <param name="a">첫 번째 아이템의 인덱스</param>
    /// <param name="b">두 번째 아이템의 인덱스</param>
    public void Swap(int a, int b)
    {
        if (a == b) return; // 같은 위치면 무시
        if (a < 0 || b < 0 || a >= items.Count || b >= items.Count) return; // 범위 체크
        
        // 튜플을 사용한 값 교환 (C# 7.0+)
        (items[a], items[b]) = (items[b], items[a]);
        OnChanged?.Invoke(); // UI 업데이트 알림
    }
}
