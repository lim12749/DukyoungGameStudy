using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor{
public class Wapon : MonoBehaviour
{
    public int id;
    public int prefabID;
    public float damage;
    public int count;
    public float speed;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (id)
        {
            case 0:
            transform.Rotate(Vector3.forward*speed*Time.deltaTime);
                break;
            default:
                break;
        }
        
        //test
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("?");
            LevelUp(20, 5);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;
        
        if(id ==0) //속성 변경과 동시에 근접무기의 경우 배치도 필요하니 함수 호출
            Batch();
    }
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = -150f;
                Batch();
                break;
            default:
                break;
        }
    }

    public void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instace.pool.Get(prefabID).transform;
                bullet.parent = transform;
            }
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
            
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f,Space.World);
            bullet.GetComponent<Bullet>().Init(damage,-1); //-1 is Infinity 관통
        }
    }
}
}
