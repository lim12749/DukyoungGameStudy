using UnityEngine;

public class ExpReceiver : MonoBehaviour
{
    public int currentExp = 0;
    public int level = 1;
    public int expToNextLevel = 100;

    public LevelUPUI levelUpUI; // UI 참조 연결 필요

    void Update()
    {
                if (Input.GetKeyDown(KeyCode.U))    
            {
                levelUpUI.ShowUpgradeOptions(); // 테스트용
            }    
    }
    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"경험치추가 {currentExp}");
        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        //레벨업인거 말해주기
        Debug.Log("레벨업");
        currentExp -= expToNextLevel;
        level++;

        // 다음 레벨 필요 경험치 증가 (선택)
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);

        // UI 열기
        //levelUpUI.ShowUpgradeOptions();

        // 일시 정지 등도 고려 가능
        Time.timeScale = 0f;
    }
}
