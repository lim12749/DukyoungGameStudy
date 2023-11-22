using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.UIElements;
using UnityEngine;

namespace Survivor{
    public class Spawner : MonoBehaviour
    {
        public SpwanData[] spwanDatas; //포멧을 만들어두고
        public Transform[] spawnPoint;
        private int m_level;
        float timer;

        private void Awake()
        {
            spawnPoint= GetComponentsInChildren<Transform>();
        }
        void Update()
        {
            timer += Time.deltaTime;
            m_level = Mathf.Min( Mathf.FloorToInt( GameManager.instace.gameTime / 10f), spwanDatas.Length - 1); //반내미
            if(timer > (spwanDatas[m_level].SpwanTime))
            {
                timer = 0;
                Spawn();
            }
        }
        private void Spawn()
        {
            GameObject enemy = GameManager.instace.pool.Get(0);
            enemy.transform.position = spawnPoint[Random.RandomRange(1, spawnPoint.Length)].position;
            enemy.GetComponent<Enemy>().Init(spwanDatas[m_level]);
        }
    }
}

[System.Serializable]
public class SpwanData
{
    public float SpwanTime;
    public int SpriteType;
    public float Health;
    public float Speed;
}

