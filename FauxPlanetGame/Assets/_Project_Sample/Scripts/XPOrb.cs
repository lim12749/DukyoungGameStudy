using UnityEngine;

public class XPOrb : MonoBehaviour
{
public int xp = 1;                   // 획득량
    public float attractRange = 6f, speed = 10f;

    Transform player;
    void Start() => player = GameObject.FindWithTag("Player").transform;

    void Update()
    {
        //씬에 XPOrb 하나 직접 놓고 플레이 → 가까이 가면 빨려와서 사라진다.
        if (!player) return;
        if (Vector3.Distance(transform.position, player.position) < attractRange)
            transform.position = Vector3.MoveTowards(transform.position,
                                                     player.position,
                                                     speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            LevelSystem.I?.GainXP(xp);   // 다음 Step 에서 만듦
            Destroy(gameObject);
        }
    }
}
