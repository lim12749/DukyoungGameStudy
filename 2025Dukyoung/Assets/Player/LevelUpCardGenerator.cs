using System.Collections.Generic;
using UnityEngine;

public struct CardData
{
    public CardKind  kind;
    public Rarity    rarity;
    public string    title;
    public string    desc;

    // Stat
    public StatType  stat;
    public int       tierValue;

    // Passive
    public PassiveType passiveType;

    // Basic attack upgrade (필요한 만큼만 사용)
    public int   multiShotAdd;
    public float projSpeedAdd;
    public float spreadAdd;
}

public static class CardTextUtil
{
    public static string StatDisplay(StatType s) => s switch
    {
        StatType.AttackDamage      => "Attack Damage",
        StatType.AttackSpeed       => "Attack Speed",
        StatType.MoveSpeed         => "Move Speed",
        StatType.CritChance        => "Crit Chance",
        StatType.CooldownReduction => "Cooldown Reduction",
        StatType.Armor             => "Armor",
        _ => s.ToString()
    };

    public static Color RarityColor(Rarity r) => r switch
    {
        Rarity.Normal    => new Color(0.85f, 0.85f, 0.85f),
        Rarity.Rare      => new Color(0.50f, 0.75f, 1.00f),
        Rarity.Epic      => new Color(0.80f, 0.55f, 1.00f),
        Rarity.Legendary => new Color(1.00f, 0.75f, 0.30f),
        _ => Color.white
    };

    public static int TierValue(Rarity r) => r switch
    {
        Rarity.Normal    => 2,
        Rarity.Rare      => 3,
        Rarity.Epic      => 4,
        Rarity.Legendary => 6,
        _ => 2
    };
}

public class LevelUpCardGenerator : MonoBehaviour
{
    [Header("Refs")]
    public PassiveAttackManager passiveMgr;
    public PlayerController     player;
    public PlayerStats          stats;

    [Header("Mix")]
    [Range(0,100)] public int attackCardChance = 30;

    [Header("Rarity Weights (%)")]
    [Range(0,100)] public int wNormal = 60;
    [Range(0,100)] public int wRare   = 25;
    [Range(0,100)] public int wEpic   = 12;
    [Range(0,100)] public int wLegend = 3;

    // ======= 외부에서 호출: 카드 3장 만들어주기 =======
    public List<CardData> Generate3()
    {
        var list = new List<CardData>(3);
        for (int i = 0; i < 3; i++) list.Add(MakeOne());
        return list;
    }

    // ======= 내부 구현 =======
    CardData MakeOne()
    {
        bool pickAttack = (Random.Range(0, 100) < attackCardChance);
        if (!pickAttack) return MakeStatCard(RandomStat());

        // 공격/패시브 후보들 구성
        var factories = new List<System.Func<CardData>>();

        // 기본공격 업글(언제나 후보 OK)
        factories.Add(MakeBasicAttackUpgradeCard);

        // 패시브들: 보유 여부에 따라 Unlock 또는 Upgrade 카드 선택
        AddPassiveFactory(factories, PassiveType.Orbitals,   MakeUnlockPassiveCard, MakeUpgradePassiveCard);
        AddPassiveFactory(factories, PassiveType.PulseNova,  MakeUnlockPassiveCard, MakeUpgradePassiveCard);
        AddPassiveFactory(factories, PassiveType.Sentry,     MakeUnlockPassiveCard, MakeUpgradePassiveCard);

        if (factories.Count == 0) return MakeStatCard(RandomStat());
        return factories[Random.Range(0, factories.Count)].Invoke();
    }

    void AddPassiveFactory(List<System.Func<CardData>> list, PassiveType t,
                           System.Func<PassiveType, CardData> unlockFactory,
                           System.Func<PassiveType, CardData> upgradeFactory)
    {
        if (!passiveMgr) return;

        // 프리팹이 등록되지 않았다면 후보 제외 (PassiveAttackManager가 알아서 관리하므로 여기서는 존재여부만 사용)
        // Unlock/Upgrade 조건은 “현재 보유중인가?”만 보면 충분.
        if (passiveMgr.Has(t)) list.Add(() => upgradeFactory(t));
        else                   list.Add(() => unlockFactory(t));
    }

    // --- Factories ---
    CardData MakeStatCard(StatType s)
    {
        var r  = RollRarity();
        int tv = CardTextUtil.TierValue(r);
        return new CardData {
            kind      = CardKind.Stat,
            rarity    = r,
            title     = $"{CardTextUtil.StatDisplay(s)} +Tier {tv}",
            desc      = StatDesc(s, tv),
            stat      = s,
            tierValue = tv
        };
    }

    CardData MakeBasicAttackUpgradeCard()
    {
        var r  = RollRarity();
        int tv = CardTextUtil.TierValue(r);
        int   addCount = Mathf.Max(1, tv / 2);
        float speedAdd = 1.0f * (tv - 1);
        float spread   = 2.0f;

        return new CardData {
            kind         = CardKind.BasicAttackUpgrade,
            rarity       = r,
            title        = $"Basic Attack Upgrade ({r})",
            desc         = $"Multishot +{addCount}, Projectile Speed +{speedAdd}, Spread +{spread}°",
            multiShotAdd = addCount,
            projSpeedAdd = speedAdd,
            spreadAdd    = spread
        };
    }

    CardData MakeUnlockPassiveCard(PassiveType t)
    {
        return new CardData {
            kind        = CardKind.PassiveUnlock,
            rarity      = RollRarity(),
            title       = $"Unlock {t}",
            desc        = $"Gain new passive: {t}",
            passiveType = t
        };
    }

    CardData MakeUpgradePassiveCard(PassiveType t)
    {
        var r  = RollRarity();
        int tv = CardTextUtil.TierValue(r);
        return new CardData {
            kind        = CardKind.PassiveUpgrade,
            rarity      = r,
            title       = $"Upgrade {t} ({r})",
            desc        = $"+Power/Scale (Tier {tv})",
            passiveType = t,
            tierValue   = tv
        };
    }

    // --- Helpers ---
    Rarity RollRarity()
    {
        int total = Mathf.Max(1, wNormal + wRare + wEpic + wLegend);
        int roll  = Random.Range(0, total);
        if (roll < wNormal) return Rarity.Normal; roll -= wNormal;
        if (roll < wRare)   return Rarity.Rare;   roll -= wRare;
        if (roll < wEpic)   return Rarity.Epic;
        return Rarity.Legendary;
    }

    StatType RandomStat()
    {
        StatType[] pool = {
            StatType.AttackDamage, StatType.AttackSpeed, StatType.MoveSpeed,
            StatType.CritChance,   StatType.CooldownReduction, StatType.Armor
        };
        return pool[Random.Range(0, pool.Length)];
    }

    string StatDesc(StatType s, int tv) => s switch
    {
        StatType.AttackDamage      => $"+{2f*tv:F0} Attack Damage",
        StatType.AttackSpeed       => $"+{0.10f*tv:F2}/s Attack Speed",
        StatType.MoveSpeed         => $"+{0.25f*tv:F2} Move Speed",
        StatType.CritChance        => $"+{(2f*tv):F0}% Crit Chance",
        StatType.CooldownReduction => $"+{(2f*tv):F0}% Cooldown Reduction",
        StatType.Armor             => $"+{1f*tv:F0} Armor",
        _ => ""
    };
}
