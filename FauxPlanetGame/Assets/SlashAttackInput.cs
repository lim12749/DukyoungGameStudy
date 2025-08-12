using UnityEngine;

public class SlashAttackInput : MonoBehaviour
{
    public SlashSpawner slashSpawner;              // 슬래시 스포너 참조
    public Animator anim;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Attack", true);
            Debug.Log("Slash Attack");
            slashSpawner.SpawnSlash(); // 또는 애니메이션 이벤트로 분리

        }
    }
}