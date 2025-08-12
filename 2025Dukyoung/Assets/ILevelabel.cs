using UnityEngine;

public interface ILevelable
{
    int Level {get;} // 현재 레벨 //get은 읽기전용 프로퍼티 이고 외부에서 값을변경할 수없습니다. 암시적으로 public 이며 외부에서 읽을 수 있습니다.
    int CurrentExp { get;} // 현재 경험치
    int ExpToNextLevel { get;} // 다음 레벨까지 필요한 경험치

    void GainExp(int exp); // 경험치 추가 (경험치 증가)  
    void LevelUp(); // 레벨 추가 (레벨 증가)
}
