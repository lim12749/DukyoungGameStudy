using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelUpPanelController : MonoBehaviour
{
    // Card types
    enum CardKind { Stat, BasicAttackUpgrade, PassiveOrbitAdd, PassiveOrbitUpgrade }

    [Header("Attack Targets")]
    [SerializeField] private PlayerController player;   // receiver for basic-attack upgrades
    [SerializeField] private OrbitalsManager orbitals;  // receiver for orbital passives

    [Header("Card Mix")]
    [Range(0,100)] public int attackUpgradeChance = 30; // % chance each slot becomes an "attack-related" card

    [Header("Panel")]
    [SerializeField] private GameObject panel; 
    [Header("Target Stats")]
    [SerializeField] private PlayerStats stats;

    [System.Serializable] struct CardUI
    {
        public Button button;
        public TMP_Text title;
        public TMP_Text desc;
        public TMP_Text rarityText;
    }
    [Header("Card UIs")]
    [SerializeField] private CardUI card1;
    [SerializeField] private CardUI card2;
    [SerializeField] private CardUI card3;

    [Header("Rarity Weights (%)")]
    [Range(0,100)] public int wNormal = 60;
    [Range(0,100)] public int wRare = 25;
    [Range(0,100)] public int wEpic = 12;
    [Range(0,100)] public int wLegendary = 3;

    readonly List<NavMeshAgent> frozenAgents = new List<NavMeshAgent>();
    bool open;

    struct CardData
    {
        public CardKind kind;
        public StatType stat; public Rarity rarity; public string title; public string desc; public int tierValue;
        // Basic-attack upgrades
        public int   multiShotAdd;
        public float projSpeedAdd;
        public float spreadAdd;
        // Orbital upgrades
        public int   orbitAddCount;
        public float orbitRadiusAdd;
        public float orbitSpeedAdd;
    }
    CardData c1, c2, c3;

    void Awake()
    {
        if (!panel) panel = gameObject;
        if (!stats) stats = FindFirstObjectByType<PlayerStats>();
        if (!player) player = FindFirstObjectByType<PlayerController>();
        if (!orbitals) orbitals = FindFirstObjectByType<OrbitalsManager>();
        panel.SetActive(false);
    }

    void OnEnable()
    {
        var xp = PlayerExperience.Instance ?? FindFirstObjectByType<PlayerExperience>();
        if (xp) { xp.onLevelUp.RemoveListener(OnLevelUp); xp.onLevelUp.AddListener(OnLevelUp); }
    }

    void OnDisable()
    {
        var xp = PlayerExperience.Instance;
        if (xp) xp.onLevelUp.RemoveListener(OnLevelUp);
    }

    void OnLevelUp(int _) => Open();

    public void Open()
    {
        if (open) return;
        open = true;
        panel.SetActive(true);

        Time.timeScale = 0f;
        FreezeAgents(true);

        Generate3Cards(out c1, out c2, out c3);
        BindCard(card1, c1);
        BindCard(card2, c2);
        BindCard(card3, c3);
    }

    public void Close()
    {
        if (!open) return;
        open = false;
        panel.SetActive(false);

        FreezeAgents(false);
        Time.timeScale = 1f;
    }

    void FreezeAgents(bool freeze)
    {
        if (freeze)
        {
            frozenAgents.Clear();
            var agents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);
            foreach (var a in agents)
            {
                if (!a) continue;
                if (!a.isStopped) frozenAgents.Add(a);
                a.isStopped = true;
            }
        }
        else
        {
            foreach (var a in frozenAgents) if (a) a.isStopped = false;
            frozenAgents.Clear();
        }
    }

    // =========================
    // ==== Card generation ====
    // =========================
    void Generate3Cards(out CardData a, out CardData b, out CardData c)
    {
        a = MakeRandomCard();
        b = MakeRandomCard();
        c = MakeRandomCard();
    }

    CardData MakeRandomCard()
    {
        bool pickAttack = (Random.Range(0, 100) < attackUpgradeChance);

        if (pickAttack)
        {
            int r = Random.Range(0, 3);
            if (r == 0) return MakeBasicAttackUpgradeCard();
            if (r == 1) return MakeOrbitAddCard();
            return MakeOrbitUpgradeCard();
        }
        else
        {
            StatType[] pool = {
                StatType.AttackDamage, StatType.AttackSpeed, StatType.MoveSpeed,
                StatType.CritChance, StatType.CooldownReduction, StatType.Armor
            };
            var s = pool[Random.Range(0, pool.Length)];
            return MakeStatCard(s);
        }
    }

    CardData MakeStatCard(StatType s)
    {
        var r  = RollRarity();
        int tv = UpgradeDefs.TierValue(r);
        string title = $"{StatDisplayEN(s)} +Tier {tv}";
        string desc  = BuildDescEN(s, tv);
        return new CardData { kind = CardKind.Stat, stat = s, rarity = r, title = title, desc = desc, tierValue = tv };
    }

    // Basic-attack upgrade card
    CardData MakeBasicAttackUpgradeCard()
    {
        var r  = RollRarity();
        int tv = UpgradeDefs.TierValue(r);      // 2..6
        int   addCount = Mathf.Max(1, tv / 2);  // 2→+1, 3→+1, 4→+2, 6→+3
        float speedAdd = 1.0f * (tv - 1);
        float spread   = 2.0f;

        return new CardData {
            kind = CardKind.BasicAttackUpgrade, rarity = r,
            title = $"Basic Attack Upgrade ({r})",
            desc  = $"Multishot +{addCount}, Projectile Speed +{speedAdd}, Spread +{spread}°",
            multiShotAdd = addCount,
            projSpeedAdd = speedAdd,
            spreadAdd    = spread
        };
    }

    // Orbital add card
    CardData MakeOrbitAddCard()
    {
        var r  = RollRarity();
        int tv = UpgradeDefs.TierValue(r);
        int add = (tv >= 5) ? 2 : 1;

        return new CardData {
            kind = CardKind.PassiveOrbitAdd, rarity = r,
            title = $"Add Orbital ({r})",
            desc  = (add == 2) ? "Add 2 orbitals (spawned opposite)" : "Add 1 orbital",
            orbitAddCount = add
        };
    }

    // Orbital upgrade card
    CardData MakeOrbitUpgradeCard()
    {
        var r  = RollRarity();
        int tv = UpgradeDefs.TierValue(r);
        float radAdd = 0.15f * tv;
        float spdAdd = 20f   * (tv - 1);

        return new CardData {
            kind = CardKind.PassiveOrbitUpgrade, rarity = r,
            title = $"Upgrade Orbitals ({r})",
            desc  = $"Radius +{radAdd:F2}, Angular Speed +{spdAdd:F0}°/s",
            orbitRadiusAdd = radAdd,
            orbitSpeedAdd  = spdAdd
        };
    }

    Rarity RollRarity()
    {
        int total = Mathf.Max(1, wNormal + wRare + wEpic + wLegendary);
        int roll = Random.Range(0, total);
        if (roll < wNormal) return Rarity.Normal; roll -= wNormal;
        if (roll < wRare)   return Rarity.Rare;   roll -= wRare;
        if (roll < wEpic)   return Rarity.Epic;
        return Rarity.Legendary;
    }

    // English stat display names
    string StatDisplayEN(StatType s)
    {
        switch (s)
        {
            case StatType.AttackDamage:      return "Attack Damage";
            case StatType.AttackSpeed:       return "Attack Speed";
            case StatType.MoveSpeed:         return "Move Speed";
            case StatType.CritChance:        return "Crit Chance";
            case StatType.CooldownReduction: return "Cooldown Reduction";
            case StatType.Armor:             return "Armor";
        }
        return s.ToString();
    }

    // English descriptions
    string BuildDescEN(StatType s, int tv)
    {
        switch (s)
        {
            case StatType.AttackDamage:      return $"+{2f*tv:F0} Attack Damage";
            case StatType.AttackSpeed:       return $"+{0.10f*tv:F2}/s Attack Speed";
            case StatType.MoveSpeed:         return $"+{0.25f*tv:F2} Move Speed";
            case StatType.CritChance:        return $"+{(2f*tv):F0}% Crit Chance";
            case StatType.CooldownReduction: return $"+{(2f*tv):F0}% Cooldown Reduction";
            case StatType.Armor:             return $"+{1f*tv:F0} Armor";
        }
        return "";
    }

    // =========================
    // ==== Bind & Apply =======
    // =========================
    void BindCard(CardUI ui, CardData cd)
    {
        if (ui.title)      ui.title.text = cd.title;
        if (ui.desc)       ui.desc.text  = cd.desc;
        if (ui.rarityText) { ui.rarityText.text = cd.rarity.ToString(); ui.rarityText.color = UpgradeDefs.RarityColor(cd.rarity); }
        if (ui.button)
        {
            ui.button.onClick.RemoveAllListeners();
            var captured = cd;
            ui.button.onClick.AddListener(() => ApplyCardAndClose(captured));
        }
    }

    void ApplyCardAndClose(CardData cd)
    {
        switch (cd.kind)
        {
            case CardKind.Stat:
                ApplyUpgrade(cd.stat, cd.tierValue);
                break;

            case CardKind.BasicAttackUpgrade:
            {
                if (!player) break;
                var ranged = player.GetComponentInChildren<RangedBasicAttack>(true);
                if (ranged)
                {
                    ranged.UpgradeMultiShot(cd.multiShotAdd);
                    ranged.UpgradeProjSpeed(cd.projSpeedAdd);
                    ranged.UpgradeSpread(cd.spreadAdd);
                }
                break;
            }

            case CardKind.PassiveOrbitAdd:
            {
                if (!orbitals) break;
                if (cd.orbitAddCount >= 2) orbitals.AddPairOpposite();
                else orbitals.AddOne();
                break;
            }

            case CardKind.PassiveOrbitUpgrade:
            {
                if (!orbitals) break;
                if (cd.orbitRadiusAdd != 0f) orbitals.UpgradeRadius(cd.orbitRadiusAdd);
                if (cd.orbitSpeedAdd  != 0f) orbitals.UpgradeSpeed(cd.orbitSpeedAdd);
                break;
            }
        }

        Close();
    }

    void ApplyUpgrade(StatType s, int tv)
    {
        if (!stats) { Close(); return; }
        switch (s)
        {
            case StatType.AttackDamage:      stats.attackDamage      += 2f * tv; break;
            case StatType.AttackSpeed:       stats.attackSpeed       += 0.10f * tv; break;
            case StatType.MoveSpeed:         stats.moveSpeed         += 0.25f * tv; break;
            case StatType.CritChance:        stats.critChance         = Mathf.Clamp01(stats.critChance + 0.02f * tv); break;
            case StatType.CooldownReduction: stats.cooldownReduction  = Mathf.Clamp01(stats.cooldownReduction + 0.02f * tv); break;
            case StatType.Armor:             stats.armor             += 1f * tv; break;
        }
    }

    void Shuffle<T>(T[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            int j = Random.Range(i, arr.Length);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
    }
}
