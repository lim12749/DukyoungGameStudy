using UnityEngine;

public enum StatType { AttackDamage, AttackSpeed, MoveSpeed, CritChance, MaxMana, CooldownReduction, Armor } // 스탯 종류

public enum Rarity { Normal, Rare, Epic, Legendary } // 등급

public static class UpgradeDefs
{
    // 등급별 “티어 값” — Normal=2, Legendary=6 (요구 사항)
    public static int TierValue(Rarity r)
    {
        switch (r)
        {
            case Rarity.Normal:    return 2;
            case Rarity.Rare:      return 3;
            case Rarity.Epic:      return 4;
            case Rarity.Legendary: return 6;
        }
        return 2;
    }

    // (선택) 등급 컬러
    public static Color RarityColor(Rarity r)
    {
        switch (r)
        {
            case Rarity.Normal:    return new Color(0.85f,0.85f,0.85f);
            case Rarity.Rare:      return new Color(0.45f,0.65f,1.00f);
            case Rarity.Epic:      return new Color(0.75f,0.45f,1.00f);
            case Rarity.Legendary: return new Color(1.00f,0.75f,0.25f);
        }
        return Color.white;
    }

    // UI표시용 간단 이름
    public static string StatDisplay(StatType s)
    {
        switch (s)
        {
            case StatType.AttackDamage:      return "공격력";
            case StatType.AttackSpeed:       return "공격속도";
            case StatType.MoveSpeed:         return "이동속도";
            case StatType.CritChance:        return "치명타 확률";
            case StatType.MaxMana:           return "최대 마나";
            case StatType.CooldownReduction: return "스킬 쿨타임 감소";
            case StatType.Armor:             return "방어력";
        }
        return s.ToString();
    }
}
