using UnityEngine;

public interface IRightClickSkill
{
    void Initialize(GameObject owner, PlayerStats stats);
    bool TryCast(); // 우클릭 시 슬롯이 호출
}
