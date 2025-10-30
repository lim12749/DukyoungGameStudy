using UnityEngine;

public enum StatType
{
    AttackDamage, AttackSpeed, MoveSpeed,
    CritChance, CooldownReduction, Armor
}
public enum Rarity { Normal, Rare, Epic, Legendary }

public enum CardKind
{
    Stat,
    BasicAttackUpgrade,
    PassiveUnlock,
    PassiveUpgrade
}

public enum PassiveType
{
    Orbitals,
    PulseNova,
    Sentry
}
