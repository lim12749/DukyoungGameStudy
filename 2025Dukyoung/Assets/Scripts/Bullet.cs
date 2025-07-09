using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f; //시간되면 삭제
    public int damage = 10;
    void Start()
    {
        Destroy(gameObject, lifeTime); //3초 뒤에 삭제
    }
    void OnCollisionEnter(Collision other)
    {
        //최신 TryGet으로 out으로 매개변수로 전달
        if (other.collider.TryGetComponent<BaseEnemy>(out var monster))
        {
            monster.TakeDamage(damage);
        }
        Destroy(gameObject); //충돌하면 삭제
        //여기에 충돌 피격 효과 이펙트도 추가
        
    }
}
