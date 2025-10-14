using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickupInteractor : MonoBehaviour
{
    public Camera cam;
    public float distance = 3f;
    public LayerMask mask = ~0;
    public Inventory inventory;

    [Header("Input (optional)")]
    public InputActionProperty pickupAction; // E
    bool _pressed;

    void OnEnable()
    {
        if (pickupAction.reference != null)
        {
            pickupAction.action.Enable();
            pickupAction.action.performed += OnPickupPressed;
        }
        if (!cam) cam = Camera.main;
    }
    
    void OnDisable()
    {
        if (pickupAction.reference != null)
            pickupAction.action.performed -= OnPickupPressed;
    }
    
    /// <summary>
    /// 픽업 액션이 수행될 때 호출되는 메서드
    /// </summary>
    /// <param name="context">입력 액션의 콜백 컨텍스트</param>
    private void OnPickupPressed(InputAction.CallbackContext context)
    {
        _pressed = true;
    }

    void Update()
    {
        if (!cam) cam = Camera.main;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out var hit, distance, mask, QueryTriggerInteraction.Ignore))
        {
            var pick = hit.collider.GetComponentInParent<PickupItem>();
            // E 키 폴백 (Input System 미연결 시)
            bool eNow = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
            if ((_pressed || eNow) && pick && inventory)
            {
                inventory.Add(pick.item);
                Destroy(pick.gameObject);
            }
        }
        _pressed = false; // 1프레임 플래그 리셋
    }

#if UNITY_EDITOR
    /// <summary>
    /// Scene 뷰에서 레이캐스트를 시각화합니다
    /// </summary>
    void OnDrawGizmos()
    {
        if (!cam) cam = Camera.main;
        if (!cam) return;

        // 카메라에서 화면 중앙으로 레이 생성
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        // 레이캐스트 실행
        if (Physics.Raycast(ray, out var hit, distance, mask, QueryTriggerInteraction.Ignore))
        {
            // 물체에 맞았을 때: 빨간색으로 표시
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin, hit.point);
            
            // 맞은 지점에 작은 구체 표시
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.1f);
            
            // 거리 표시
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(hit.point, Vector3.one * 0.2f);
        }
        else
        {
            // 물체에 맞지 않았을 때: 초록색으로 표시
            Gizmos.color = Color.green;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * distance);
        }
        
        // 카메라 위치에 작은 구체 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(ray.origin, 0.05f);
    }
#endif
}
