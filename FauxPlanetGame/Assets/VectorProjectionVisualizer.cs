using UnityEngine;

public class VectorProjectionVisualizer : MonoBehaviour
{
    public Transform cameraTransform;          // ★ MainCamera
    public float drawLength = 4f;              // ★ 그리기 길이

    private void OnDrawGizmos()
    {
        if (!cameraTransform) return;

        Vector3 camFwd = cameraTransform.forward;
        Vector3 camFwdFlat = Vector3.ProjectOnPlane(camFwd, Vector3.up).normalized;

        Gizmos.color = Color.red;                                  // 카메라 원본 forward
        Gizmos.DrawLine(transform.position,
                        transform.position + camFwd * drawLength);

        Gizmos.color = Color.green;                                // 평면 투영 + 정규화
        Gizmos.DrawLine(transform.position,
                        transform.position + camFwdFlat * drawLength);
    }
}
