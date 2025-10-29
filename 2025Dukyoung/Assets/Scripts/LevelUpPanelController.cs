using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelUpPanelController : MonoBehaviour
{
    // 카드 종류
    enum CardKind { Stat, BasicAttackUpgrade, PassiveOrbitAdd, PassiveOrbitUpgrade }

    // 적용 대상
    [Header("Attack Targets")]
    [SerializeField] private PlayerController player;   // 기본공격 업그레이드 적용 대상
    [SerializeField] private OrbitalsManager orbitals;  // 오비탈 관리 대상

    // 카드 믹스 비율
    [Header("Card Mix")]
    [Range(0,100)] public int attackUpgradeChance = 30; // 각 슬롯이 공격류 카드가 될 확률(%)

    // 패널 & 스탯
    [Header("Panel")]
    [SerializeField] private GameObject panel; // 비활성 시작
    [Header("Target Stats")]
    [SerializeField] private PlayerStats stats;

    // 카드 UI 3개
    [System.Serializable] struct CardUI
    {
        public Button button;
        public TMP_Text title;
        public TMP_Text desc;
        public TMP_Text rarityText;
        // (원하면 Image bg 추가해서 등급색 입히기 가능)
    }
    [Header("Card UIs")]
    [SerializeField] private CardUI card1;
    [SerializeField] private CardUI card2;
    [SerializeField] private CardUI card3;

    // 등급 확률
    [Header("Rarity Weights (%)")]
    [Range(0,100)] public int wNormal = 60;
    [Range(0,100)] public int wRare = 25;
    [Range(0,100)] public int wEpic = 12;
    [Range(0,100)] public int wLegendary = 3;

    readonly List<NavMeshAgent> frozenAgents = new List<NavMeshAgent>();
    bool open;

    // 카드 데이터
    struct CardData
    {
        public CardKind kind;
        public StatType stat; public Rarity rarity; public string title; public string desc; public int tierValue;
        // 기본공격 업글
        public int   multiShotAdd;
        public float projSpeedAdd;
        public float spreadAdd;
        // 오비탈 업글
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
    // ==== 카드 생성 로직 ====
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
            // 공격류 카드 중 택1
            int r = Random.Range(0, 3);
            if (r == 0) return MakeBasicAttackUpgradeCard(); // 멀티샷/속도/퍼짐
            if (r == 1) return MakeOrbitAddCard();           // 오비탈 추가
            return MakeOrbitUpgradeCard();                   // 오비탈 강화
        }
        else
        {
            // 스탯 카드
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
        var r = RollRarity();
        int tv = UpgradeDefs.TierValue(r);
        string title = $"{UpgradeDefs.StatDisplay(s)} +{tv}티어";
        string desc  = BuildDesc(s, tv);
        return new CardData { kind = CardKind.Stat, stat = s, rarity = r, title = title, desc = desc, tierValue = tv };
    }

    // 기본공격 업그레이드 카드(등급에 따라 수치 튜닝)
    CardData MakeBasicAttackUpgradeCard()
    {
        var r  = RollRarity();
        int tv = UpgradeDefs.TierValue(r); // 2~6
        int   addCount = Mathf.Max(1, tv / 2); // 2→+1, 3→+1, 4→+2, 6→+3
        float speedAdd = 1.0f * (tv - 1);      // 투사체 속도 보너스
        float spread   = 2.0f;                 // 퍼짐 소폭 증가

        return new CardData {
            kind = CardKind.BasicAttackUpgrade, rarity = r,
            title = $"기본공격 업그레이드 ({r})",
            desc  = $"멀티샷 +{addCount}, 투사체 속도 +{speedAdd}, 퍼짐 +{spread}°",
            multiShotAdd = addCount,
            projSpeedAdd = speedAdd,
            spreadAdd    = spread
        };
    }

    // 오비탈 추가 카드
    CardData MakeOrbitAddCard()
    {
        var r  = RollRarity();
        int tv = UpgradeDefs.TierValue(r);
        int add = (tv >= 5) ? 2 : 1; // 높은 등급일수록 2개 추가

        return new CardData {
            kind = CardKind.PassiveOrbitAdd, rarity = r,
            title = $"오비탈 추가 ({r})",
            desc  = (add == 2) ? "오비탈 2개 추가(반대 위치 생성)" : "오비탈 1개 추가",
            orbitAddCount = add
        };
    }

    // 오비탈 강화 카드(반경/속도)
    CardData MakeOrbitUpgradeCard()
    {
        var r  = RollRarity();
        int tv = UpgradeDefs.TierValue(r);
        float radAdd = 0.15f * tv;
        float spdAdd = 20f   * (tv - 1);

        return new CardData {
            kind = CardKind.PassiveOrbitUpgrade, rarity = r,
            title = $"오비탈 강화 ({r})",
            desc  = $"반경 +{radAdd:F2}, 회전속도 +{spdAdd:F0}°/s",
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

    // 스탯 카드 설명 포맷
    string BuildDesc(StatType s, int tv)
    {
        switch (s)
        {
            case StatType.AttackDamage:      return $"attack +{2f*tv:F0}";
            case StatType.AttackSpeed:       return $"attack Speed +{0.10f*tv:F2}/s";
            case StatType.MoveSpeed:         return $"MoveSpeed +{0.25f*tv:F2}";
            case StatType.CritChance:        return $"Crital +{(2f*tv):F0}%";
            case StatType.CooldownReduction: return $"CoolTime - +{(2f*tv):F0}%";
            case StatType.Armor:             return $"Defence +{1f*tv:F0}";
        }
        return "";
    }

    // =========================
    // ==== 바인딩 & 적용 ====
    // =========================
    void BindCard(CardUI ui, CardData cd)
    {
        if (ui.title)      ui.title.text = cd.title;
        if (ui.desc)       ui.desc.text  = cd.desc;
        if (ui.rarityText) { ui.rarityText.text = cd.rarity.ToString(); ui.rarityText.color = UpgradeDefs.RarityColor(cd.rarity); }
        if (ui.button)
        {
            ui.button.onClick.RemoveAllListeners();
            var captured = cd; // 캡처 안전
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
