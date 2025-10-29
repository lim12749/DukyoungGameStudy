using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;  // 씬의 게임오버 패널(비활성 시작 권장)

    void Awake()
    {
        if (gameOverUI) gameOverUI.SetActive(false); // 씬 시작 시 항상 꺼두기
    }

    public void ShowGameOverPanel()
    {
        if (gameOverUI) gameOverUI.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        if (gameOverUI) gameOverUI.SetActive(false);
    }

    // UI 버튼에 직접 연결하고 싶을 때 사용
    public void OnClick_Restart()
    {
        GameManagerTower.RestartGameStatic();
    }
}
