using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelUpPanelController : MonoBehaviour
{
    [Header("Refs")]
    public LevelUpCardGenerator generator;       // 위 3번 스크립트
    public PassiveAttackManager passiveMgr;      // 플레이어에 붙은 매니저
    public PlayerController     player;          // 기본공격 업그레이드 적용 대상
    public PlayerStats          stats;

    [Header("Panel")]
    public GameObject panel;

    [System.Serializable] public struct CardUI
    {
        public Button   button;
        public TMP_Text title;
        public TMP_Text desc;
        public TMP_Text rarityText;
    }
    [Header("3 Card Slots")]
    public CardUI card1, card2, card3;

    readonly List<NavMeshAgent> frozenAgents = new();
    bool open;
    List<CardData> curCards;

    void Awake()
    {
        if (!panel) panel = gameObject;
        if (!generator)    generator   = FindFirstObjectByType<LevelUpCardGenerator>();
        if (!passiveMgr)   passiveMgr  = FindFirstObjectByType<PassiveAttackManager>();
        if (!player)       player      = FindFirstObjectByType<PlayerController>();
        if (!stats)        stats       = FindFirstObjectByType<PlayerStats>();
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
        open = true; panel.SetActive(true);

        Time.timeScale = 0f; FreezeAgents(true);

        curCards = generator.Generate3();
        BindCard(card1, curCards[0], 0);
        BindCard(card2, curCards[1], 1);
        BindCard(card3, curCards[2], 2);
    }

    public void Close()
    {
        if (!open) return;
        open = false; panel.SetActive(false);

        FreezeAgents(false); Time.timeScale = 1f;
    }

    void FreezeAgents(bool freeze)
    {
    if (freeze)
    {
        frozenAgents.Clear();
        var agents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);

        foreach (var a in agents)
        {
            if (!IsUsableAgent(a)) continue;           // 안전가드

            // 이미 멈춰있지 않은 애만 기록하고 멈춘다
            if (!a.isStopped) frozenAgents.Add(a);
            a.isStopped = true;
        }
    }
    else
    {
        // 저장해둔 애들만 원복, 중간에 파괴/비활성 되었을 수 있으므로 재검사
        foreach (var a in frozenAgents)
        {
            if (!IsUsableAgent(a)) continue;
            a.isStopped = false;
        }
        frozenAgents.Clear();
    }
}

    // 보조: isStopped를 안전하게 호출할 수 있는지 검사
    static bool IsUsableAgent(NavMeshAgent a)
    {
        // null 아님, 컴포넌트 enable, 오브젝트 활성, 실제 NavMesh 위에 배치
        return a != null
            && a.isActiveAndEnabled
            && a.enabled
            && a.gameObject.activeInHierarchy
            && a.isOnNavMesh;
    }
    void BindCard(CardUI ui, CardData cd, int index)
    {
        if (ui.title)      ui.title.text = cd.title;
        if (ui.desc)       ui.desc.text  = cd.desc;
        if (ui.rarityText) { ui.rarityText.text = cd.rarity.ToString(); ui.rarityText.color = CardTextUtil.RarityColor(cd.rarity); }

        if (ui.button)
        {
            ui.button.onClick.RemoveAllListeners();
            ui.button.onClick.AddListener(() => ApplyAndClose(index));
        }
    }

    void ApplyAndClose(int index)
    {
        var cd = curCards[index];

        switch (cd.kind)
        {
            case CardKind.Stat:
                ApplyStat(cd.stat, cd.tierValue);
                break;

            case CardKind.BasicAttackUpgrade:
                var ranged = player ? player.GetComponentInChildren<RangedBasicAttack>(true) : null;
                if (ranged)
                {
                    ranged.UpgradeMultiShot(cd.multiShotAdd);
                    ranged.UpgradeProjSpeed(cd.projSpeedAdd);
                    ranged.UpgradeSpread(cd.spreadAdd);
                }
                break;

            case CardKind.PassiveUnlock:
                passiveMgr?.Unlock(cd.passiveType);
                break;

            case CardKind.PassiveUpgrade:
                passiveMgr?.Upgrade(cd.passiveType, 1);
                break;
        }

        Close();
    }

    void ApplyStat(StatType s, int tv)
    {
        if (!stats) return;
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
}
