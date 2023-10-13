using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    //original MonsterModel
    public Monster Monsterprefab;

    //MonsterSpawnPoint
    public Transform[] spawnPoints;

    public float damageMax = 40f;
    public float damageMin = 10f;

    public float healthMax = 200f;
    public float healthmin = 50f;

    public float SpeedMax = 3f;
    public float SpeedMin = 1f;

    public Color strongMonsterColor = Color.red;

    private List<Monster> monsters = new List<Monster>(); //creating Monster addList
    private int wave; //Round;

    private void Update()
    {
        //EndGame No Create Monster
        if(GameManager.instance != null && GameManager.instance.isGameOver)
        {
            return;
        }

        if(monsters.Count <=0)
        {
            //CreateMonster
            SpawnWave();
        }
    }
    
    private void SpawnWave()
    {
        wave++; //wave count;
        // spwan Count = nowWave * 1.5 (Round) 
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        
        for(int i=0; i<spawnCount; i++)
        {
            //powerProgress
            float enemyIntensity = UnityEngine.Random.Range(0f, 1f);

           
        }

       
        //이게 어떤거를 했는지 알아야 책정을 하시지 않냐 프로그램 개발이 들어간건지
        //
    }
    private void CreateMonster(float _intensity)
    {
        float _health = Mathf.Lerp(healthmin, healthMax, _intensity);
        float _damage = Mathf.Lerp(damageMin, damageMin, _intensity);
        float _speed = Mathf.Lerp(SpeedMax, SpeedMax, _intensity);

        Transform _spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        Monster _monster = Instantiate(Monsterprefab, _spawnPoint.position, _spawnPoint.rotation);

        _monster.SetUp(_health, _damage, _speed);//setting value

        monsters.Add(_monster); //listAdd

        //addEvnet //NoNameMethord //Ramda
        _monster.OnDeath+=() => monsters.Remove(_monster);
        _monster.OnDeath += () => Destroy(_monster.gameObject, 3f);
        _monster.OnDeath += () => GameManager.instance.AddScore(10);
        //_monster.OnDeath += () => GameManager.Destroy(_monster);



    }
}
 