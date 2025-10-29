using UnityEngine;

public class RingPulseVFX : MonoBehaviour
{
    public static void Spawn(Vector3 center, float startRadius, float endRadius, float duration, Color color)
    {
        var go = new GameObject("VFX_RingPulse");
        go.transform.position = center + Vector3.up * 0.05f; // 바닥에서 살짝 띄움
        var vfx = go.AddComponent<RingPulseVFX>();
        vfx.Setup(startRadius, endRadius, duration, color);
    }

    const int SEGMENTS = 48;
    LineRenderer lr;
    float r0, r1, t, dur;
    Color col;

    void Setup(float startR, float endR, float duration, Color c)
    {
        r0 = Mathf.Max(0.01f, startR);
        r1 = Mathf.Max(r0, endR);
        dur = Mathf.Max(0.05f, duration);
        col = c;

        lr = gameObject.AddComponent<LineRenderer>();
        lr.positionCount = SEGMENTS + 1;      // 폐곡선
        lr.loop = true;
        lr.widthMultiplier = 0.08f;
        lr.useWorldSpace = true;
        lr.numCornerVertices = 2;
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
        lr.material = new Material(Shader.Find("Sprites/Default")); // 기본 투명
        lr.startColor = lr.endColor = col;

        UpdateRing(r0);
    }

    void Update()
    {
        t += Time.deltaTime;
        float k = Mathf.Clamp01(t / dur);
        // 반경/알파 보간
        float r = Mathf.Lerp(r0, r1, k);
        Color c = col; c.a = Mathf.Lerp(1f, 0f, k);
        lr.startColor = lr.endColor = c;

        UpdateRing(r);

        if (t >= dur) Destroy(gameObject);
    }

    void UpdateRing(float radius)
    {
        for (int i = 0; i <= SEGMENTS; i++)
        {
            float ang = (i / (float)SEGMENTS) * Mathf.PI * 2f;
            Vector3 p = new Vector3(Mathf.Cos(ang) * radius, 0f, Mathf.Sin(ang) * radius);
            lr.SetPosition(i, transform.position + p);
        }
    }
}
