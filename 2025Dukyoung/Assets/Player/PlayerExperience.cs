using UnityEngine;
using UnityEngine.Events;

public class PlayerExperience : MonoBehaviour
{
    public static PlayerExperience Instance { get; private set; }

    [Header("Rule")]
    public int baseExpToLevel = 10;   // 1→2 필요치
    public int perLevelIncrease = 5;  // 레벨당 증가량

    [Header("State (ReadOnly)")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int expToNext = 10;

    public int Level => level;
    public int CurrentExp => currentExp;
    public int ExpToNext => expToNext;

    [Header("Events")]
    public UnityEvent<int,int> onExpChanged; // (current, toNext)
    public UnityEvent<int> onLevelUp;        // (newLevel)

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        expToNext = CalcExpToNext(level);
        onExpChanged?.Invoke(currentExp, expToNext);
    }

    int CalcExpToNext(int lv) => Mathf.Max(1, baseExpToLevel + (lv - 1) * perLevelIncrease);

    public void AddExp(int amount)
    {
        if (amount <= 0) return;
        currentExp += amount;

        // 여러 번 레벨업도 처리
        while (currentExp >= expToNext)
        {
            currentExp -= expToNext;
            level++;
            onLevelUp?.Invoke(level);         //레벨업
            expToNext = CalcExpToNext(level); //다음 레벨 경험치 갱신
        }
        onExpChanged?.Invoke(currentExp, expToNext);
    }

    public void ResetExpLine(int newLevel = 1)
    {
        level = Mathf.Max(1, newLevel);
        currentExp = 0;
        expToNext = CalcExpToNext(level);
        onExpChanged?.Invoke(currentExp, expToNext); //경험치
    }
}
