using UnityEngine;
using System;
public class LevelSystem : MonoBehaviour
{
    public static LevelSystem I { get; private set; }

    public int level = 1, xp = 0, nextXP = 5;
    public event Action<int> OnLevelUp;

    void Awake() => I = this;

    public void GainXP(int amount)
    {
        xp += amount;
        Debug.Log($"GainXP 호출: +{amount}, 현재 XP {xp}/{nextXP}");
        if (xp >= nextXP)
        {
            xp -= nextXP;
            level++;
            nextXP = Mathf.CeilToInt(nextXP * 1.5f);

            Debug.Log($"Level UP! → {level}");
            OnLevelUp?.Invoke(level);
        }
    }
}
