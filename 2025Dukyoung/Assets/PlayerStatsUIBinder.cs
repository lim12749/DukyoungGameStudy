using UnityEngine;
using TMPro;

public class PlayerStatsUIBinder : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private TMP_Text text;

    void Awake()
    {
        if (!stats) stats = FindFirstObjectByType<PlayerStats>();
        if (!text)  text  = GetComponentInChildren<TMP_Text>(true);
    }

    void Update()
    {
        if (!stats || !text) return;
        text.text =
            $"공격력: {stats.attackDamage:F1}\n" +
            $"공격속도: {stats.attackSpeed:F2}/s\n" +
            $"이동속도: {stats.moveSpeed:F1}\n" +
            $"치확: {(stats.critChance*100f):F0}%\n" +
            $"쿨감: {(stats.cooldownReduction*100f):F0}%\n" +
            $"방어력: {stats.armor:F1}";
    }
}
