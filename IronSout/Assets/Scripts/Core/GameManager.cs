using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임의 전체 진행 및 점수, 게임오버, 재시작 등을 관리하는 매니저
/// 싱글톤 패턴으로 구현되어 게임 전체에서 하나의 인스턴스만 존재합니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static int AnimatorTransi;  
    /// <summary>
    /// 싱글톤 인스턴스
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// 현재 게임 점수
    /// </summary>
    public int CurrentScore { get; private set; }

    /// <summary>
    /// 게임 오버 상태 여부
    /// </summary>
    public bool IsGameOver { get; private set; }

    /// <summary>
    /// 플레이어 참조 (재시작 시 초기화용)
    /// </summary>
    [SerializeField] PlayerHealth playerHealthComponent;

    /// <summary>
    /// 초기화 함수 - 싱글톤 설정 및 게임 상태 초기화
    /// </summary>
    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance != null) 
        { 
            Destroy(gameObject); 
            return; 
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 게임 상태 초기화
        ResetGameState();
    }

    /// <summary>
    /// 게임 상태를 초기화하는 함수
    /// </summary>
    public void ResetGameState()
    {
        
        // 시간 스케일을 정상으로 복원
        Time.timeScale = 1f;
        
        // 게임 상태 초기화
        IsGameOver = false;
        CurrentScore = 0;
        
        // 플레이어 체력 초기화
        ResetPlayerHealth();
    }

    /// <summary>
    /// 플레이어 체력을 초기화하는 함수
    /// </summary>
    void ResetPlayerHealth()
    {
        
        // 플레이어 참조가 없으면 자동으로 찾기
        if (playerHealthComponent == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                playerHealthComponent = playerObject.GetComponent<PlayerHealth>();
            }
        }
        
        // 플레이어 체력 초기화
        if (playerHealthComponent != null)
        {
            playerHealthComponent.ResetPlayerDeathFlags();
        }
    }

    /// <summary>
    /// 점수를 추가하는 함수
    /// </summary>
    /// <param name="scoreToAdd">추가할 점수</param>
    public void AddScore(int scoreToAdd)
    {
        // 게임오버 상태에서는 점수 추가 불가
        if (IsGameOver) 
        {
            return;
        }
        
        CurrentScore += scoreToAdd;
        
        // HUD에 점수 업데이트 알림
        if (HUD.Instance != null)
        {
            HUD.Instance.SetScore(CurrentScore);
        }
    }

    /// <summary>
    /// 플레이어에게 데미지를 적용하는 함수 (임시 - 실제로는 PlayerHealth에서 직접 처리)
    /// </summary>
    /// <param name="damageAmount">적용할 데미지</param>
    public void DamagePlayer(int damageAmount)
    {
        // 게임오버 상태에서는 데미지 적용 불가
        if (IsGameOver) 
        {
            return;
        }
        
        // 플레이어 체력 컴포넌트에 직접 데미지 적용
        if (playerHealthComponent != null)
        {
            playerHealthComponent.TakeDamage(damageAmount);
        }
        else
        {
            Debug.LogWarning("[GameManager] PlayerHealth 컴포넌트를 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// 게임 오버 처리를 수행하는 함수
    /// </summary>
    public void GameOver()
    {
        // 이미 게임오버 상태라면 중복 처리 방지
        if (IsGameOver) 
        {
            return;
        }
        
        IsGameOver = true;
        
        // 게임 시간을 정지
        Time.timeScale = 0f;
        
        // HUD에 게임오버 패널 표시 요청
        if (HUD.Instance != null)
        {
            HUD.Instance.ShowGameOverPanel(CurrentScore);
        }
        
        Debug.Log("[GameManager] Game Over! Final Score: " + CurrentScore);
    }

    /// <summary>
    /// 게임을 재시작하는 함수 (씬 리로드)
    /// </summary>
    public void Retry()
    {
        Debug.Log("[GameManager] Retrying game...");
        
        // 게임 상태 초기화
        ResetGameState();
        
        // 현재 씬을 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
