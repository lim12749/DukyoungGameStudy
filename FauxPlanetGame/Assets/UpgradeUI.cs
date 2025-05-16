using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeUI : MonoBehaviour
{
    [Header("Data")]
    public StatUpgrade[] upgradePool;      // ➜ 3개 SO drag

    [Header("UI")]
    public GameObject panel;               // UpgradePanel
    public Button[] buttons;               // 3개 버튼

    PlayerStates stats;

    void Start()
    {
        stats = FindFirstObjectByType<PlayerStates>();
        panel.SetActive(false);

        /* Level Up 이벤트 구독 */
        LevelSystem.I.OnLevelUp += ShowPanel;
    }

    /* 레벨업 때 호출 */
    void ShowPanel(int lvl)
    {
        panel.SetActive(true);
        Time.timeScale = 0f;               // 게임 일시정지

        for (int i = 0; i < buttons.Length; i++)
        {
            /* 풀에서 랜덤 선택 */
            StatUpgrade up = upgradePool[Random.Range(0, upgradePool.Length)];

            /* 버튼 텍스트 세팅 */
            TMP_Text txt = buttons[i].GetComponentInChildren<TMP_Text>();
            txt.text = up.upgradeName;

            /* 클릭 리스너 등록 */
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => { Apply(up); });
        }
    }

    void Apply(StatUpgrade up)
    {
        stats.ApplyUpgrade(up);     // 능력치 반영
        panel.SetActive(false);
        Time.timeScale = 1f;        // 재개
    }
}
