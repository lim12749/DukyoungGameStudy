using UnityEngine;

public interface IPassiveAttack
{
    void Initialize(GameObject owner, PlayerStats stats); // 소유자/스탯 주입
    void Upgrade(int levelDelta);                         // 레벨 or 위상 업그레이드
    string DisplayName { get; }                           // (UI 표시용)
}