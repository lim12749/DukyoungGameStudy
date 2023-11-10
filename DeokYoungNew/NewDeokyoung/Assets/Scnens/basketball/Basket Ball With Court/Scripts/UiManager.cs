using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour {

    private static UiManager m_instance; // 싱글톤이 할당될 변수
    public static UiManager instance
    {
        get
        {
            if(m_instance == null )
            {
                m_instance = FindObjectOfType<UiManager>();
            }
            return m_instance;
        }
    }

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI WaveCountText;
    public TextMeshProUGUI LiveMonsterCount;
    public GameObject GameOverUI;

    public void UpdateScoreText(int newScore)
    {
        ScoreText.text = "Score : " + newScore;
    }
    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        WaveCountText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }
    public void UpdateLiveMonster(int waves, int count)
    {
        LiveMonsterCount.text = "Enemy Left : " + count;
    }
    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        GameOverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
