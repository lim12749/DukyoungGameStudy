using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTower : MonoBehaviour
{
    public static GameManagerTower Instance { get; private set; }
    public bool IsGameOver => _gameOver;

    [Header("Refs")]
    [SerializeField] private Tower tower;       // Tower.onLose 구독 대상
    [SerializeField] private UIManager ui;      // 씬의 UIManager (태그 필요 없음)

    bool _gameOver;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지

        if (!tower) tower = FindFirstObjectByType<Tower>();
        if (!ui)    ui    = FindFirstObjectByType<UIManager>();
    }

    void OnEnable()
    {
        if (tower) tower.onLose.AddListener(TriggerGameOver);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        if (tower) tower.onLose.RemoveListener(TriggerGameOver);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void TriggerGameOver()
    {
        if (_gameOver) return;
        _gameOver = true;

        ui?.ShowGameOverPanel();
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        _gameOver = false;
        ui?.HideGameOverPanel();  // 씬 리로드 전 UI 끄기

        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새 씬의 참조 재바인딩
        BindTower(FindFirstObjectByType<Tower>());
        ui = FindFirstObjectByType<UIManager>();

        // 새 씬 시작 시 UI는 항상 꺼둠
        ui?.HideGameOverPanel();
    }

    public void BindTower(Tower newTower)
    {
        if (tower == newTower) return;

        if (tower != null)
            tower.onLose.RemoveListener(TriggerGameOver);

        tower = newTower;

        if (tower != null)
            tower.onLose.AddListener(TriggerGameOver);
    }

    public static void RestartGameStatic()     => Instance?.RestartGame();
    public static void TriggerGameOverStatic() => Instance?.TriggerGameOver();
}
