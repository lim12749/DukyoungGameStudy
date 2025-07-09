using UnityEngine;

public class TestEnemy : BaseEnemy
{
    protected override void Attack()
    {
        Debug.Log("🐺 늑대가 플레이어를 물었습니다!");
    }
}
