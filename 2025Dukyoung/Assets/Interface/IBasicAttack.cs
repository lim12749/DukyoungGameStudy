using UnityEngine;

public interface IBasicAttack
{
    void Initialize(GameObject owner, PlayerStats stats); // 공격자 및 스탯 정보 설정
    bool TryAttack(Transform target); // 쿨타임/데미지 포함 처리
}