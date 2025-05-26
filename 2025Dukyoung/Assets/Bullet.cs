using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f; //시간되면 삭제

    void Start()
    {
        Destroy(gameObject, lifeTime); //3초 뒤에 삭제
    }
    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject); //충돌하면 삭제
        //여기에 충돌 피격 효과 이펙트도 추가
    }
}
