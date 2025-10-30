using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [Header("Refs")]
    public TextMeshPro text;          // 프리팹에 TMP_Text 연결
    public Camera cam;             // 비워두면 자동(MainCamera)

    [Header("VFX")]
    public float riseDistance = 1.0f;  // 떠오르는 높이
    public float lifetime = 0.8f;      // 총 지속 시간(초)
    public float startScale = 1.0f;
    public float endScale = 0.8f;
    public Color normalColor = Color.white;
    public Color critColor   = Color.red;

    Vector3 startPos, endPos;
    float t;

    public void Show(Vector3 worldPos, float amount, bool isCrit)
    {
        if (!text) text = GetComponentInChildren<TextMeshPro>(true);
        if (!cam)  cam  = Camera.main;

        startPos = worldPos;
        endPos   = worldPos + Vector3.up * riseDistance;
        t = 0f;

        // 텍스트/색상
        text.text = Mathf.RoundToInt(amount).ToString();
        text.color = isCrit ? critColor : normalColor;

        // 시작 위치/스케일
        transform.position = startPos;
        transform.localScale = Vector3.one * startScale;

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!cam) cam = Camera.main;

        t += Time.unscaledDeltaTime;   // 일시정지 영향 안 받게(레벨업 패널때도 사라지도록)
        float u = Mathf.Clamp01(t / lifetime);

        // 위치/스케일/알파
        transform.position = Vector3.Lerp(startPos, endPos, u);
        float scale = Mathf.Lerp(startScale, endScale, u);
        transform.localScale = Vector3.one * scale;

        var c = text.color;
        c.a = 1f - u;
        text.color = c;

        // 카메라 바라보기(빌보드)
        if (cam)
        {
            Vector3 fwd = (transform.position - cam.transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);
        }

        if (u >= 1f) Destroy(gameObject);
    }
}
