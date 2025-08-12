using UnityEngine;
using UnityEngine.UI;
public class LevelUPUI : MonoBehaviour
{
    public GameObject panel;
    public Button[] optionButtons;

    public void ShowUpgradeOptions()
    {
        panel.SetActive(true);

        // 기존 리스너 제거 후 새로운 리스너 연결
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // 캡처용
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => ChooseOption(index));
        }

        Time.timeScale = 0f; // 일시정지
    }

    public void ChooseOption(int index)
    {
        //FindObjectOfType<PlayerStats>().Upgrade(index);

        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
