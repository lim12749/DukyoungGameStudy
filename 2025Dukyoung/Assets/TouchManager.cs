using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    [Header("Input (PlayerInput 액션 이름 그대로)")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string touchPositionActionName = "TouchPosition"; // Vector2
    [SerializeField] private string touchPressActionName    = "TouchPress";    // Button
    [SerializeField] private GameObject gameObjectsToActivate;
        
    [Header("Tuning")]
    [Tooltip("기준점에서 좌우로 떨어진 픽셀 * 이 값 * dt 만큼 회전(도/초)")]
    [SerializeField] private float sensitivity = 0.25f;  // 0.15~0.35 권장
    [Tooltip("미세 흔들림 무시 영역(픽셀)")]
    [SerializeField] private float deadZone = 2f;
    [Tooltip("회전 속도 상한(도/초). 0이면 무제한")]
    [SerializeField] private float maxDegPerSec = 360f;
    [SerializeField] private bool invert = false;

    private InputAction posAction;   // Vector2
    private InputAction pressAction; // Button

    private bool dragging;
    private float anchorX; // 눌렀을 때의 화면 x좌표(픽셀)

    void Awake()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        posAction   = playerInput.actions[touchPositionActionName];
        pressAction = playerInput.actions[touchPressActionName];
    }

    void OnEnable()
    {
        posAction.Enable();
        pressAction.Enable();
        pressAction.started  += OnPressStarted;
        pressAction.canceled += OnPressCanceled;
    }

    void OnDisable()
    {
        pressAction.started  -= OnPressStarted;
        pressAction.canceled -= OnPressCanceled;
        posAction.Disable();
        pressAction.Disable();
    }

    void OnPressStarted(InputAction.CallbackContext _)
    {
        // 기준점(앵커) 설정: 누른 순간의 x 픽셀 좌표
        Vector2 p = posAction.ReadValue<Vector2>();
        anchorX = p.x;
        dragging = true;
    }

    void OnPressCanceled(InputAction.CallbackContext _)
    {
        dragging = false;
    }

    void Update()
    {
        if (!dragging || !pressAction.IsPressed()) return;

        Vector2 p = posAction.ReadValue<Vector2>();
        float dx = p.x - anchorX;               // 기준점 대비 현재 x 차이
        if (Mathf.Abs(dx) < deadZone) return;   // 데드존

        // dx가 +면 우회전, -면 좌회전
        float dir = Mathf.Sign(dx);
        float speed = Mathf.Abs(dx) * sensitivity;       // 도/초
        if (maxDegPerSec > 0f) speed = Mathf.Min(speed, maxDegPerSec);
        if (invert) dir = -dir;

        float deltaYaw = dir * speed * Time.deltaTime;   // 이번 프레임 회전량
         gameObjectsToActivate.transform.Rotate(0f, -deltaYaw, 0f, Space.Self);
    }
}
