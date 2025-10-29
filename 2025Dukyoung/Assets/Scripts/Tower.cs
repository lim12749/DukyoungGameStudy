using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    [Header("진행/패배 설정")]
    [Tooltip("이 값(%)에 도달하면 패배")]
    [Range(1, 100)] public float loseAt = 100f;

    [Header("상태 (읽기 전용 표기)")]
    [Range(0, 100)] [SerializeField] private float progress = 0f;
    public float Progress => progress;          // 현재 %
    public float Progress01 => progress / loseAt; // 0~1 정규화
    public bool IsLost  { get; private set; }

    [Header("이벤트")]
    public UnityEvent<float> onProgressChanged; // 현재 % 전달
    public UnityEvent onLose;                   // 패배 시 1회 호출

    void Start()
    {
      onProgressChanged?.Invoke(Progress); // 시작 시 현재값을 한 번 쏴서 UI 초기화  
    }

    /// <summary>KillZone 등에서 호출: 진행도를 amount(%)만큼 증가.</summary>
    public void AddProgress(float amount)
    {
        if (IsLost || amount <= 0f) return;

        float before = progress;
        progress = Mathf.Clamp(progress + amount, 0f, loseAt);

        if (!Mathf.Approximately(before, progress))
            onProgressChanged?.Invoke(progress);

        if (progress >= loseAt && !IsLost)
        {
            IsLost = true;
            onLose?.Invoke();
            // 필요하면 여기서 게임오버 처리(패널 오픈/시간정지/씬전환 등)
        }
    }

    /// <summary>진행도/패배상태 초기화.</summary>
    public void ResetProgress(float startValue = 0f)
    {
        IsLost = false;
        progress = Mathf.Clamp(startValue, 0f, loseAt);
        onProgressChanged?.Invoke(progress);
    }

    /// <summary>임계치 변경(변경 후 진행도 이벤트 갱신).</summary>
    public void SetLoseAt(float newLoseAt)
    {
        loseAt = Mathf.Clamp(newLoseAt, 1f, 100f);
        progress = Mathf.Min(progress, loseAt);
        onProgressChanged?.Invoke(progress);
    }
}
