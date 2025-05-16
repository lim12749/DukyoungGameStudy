using UnityEngine;

public class CursorController : MonoBehaviour
{
   private void Start() => LockCursor();

    private void Update()
    {
        /* ESC → 커서 해제 & 보이기 */
        if (Input.GetKeyDown(KeyCode.Escape))
            UnlockCursor();

        /* 마우스 왼쪽 클릭 → 다시 잠금 & 숨김 */
        if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
            LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 화면 중앙 고정
        Cursor.visible   = false;                   // 커서 숨김
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;     // 자유 이동
        Cursor.visible   = true;                    // 커서 표시
    }
}
