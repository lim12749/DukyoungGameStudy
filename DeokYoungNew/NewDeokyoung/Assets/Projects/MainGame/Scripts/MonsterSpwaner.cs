using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpwaner : MonoBehaviour
{
    public Monster MonsterPrefab; //Orignal

    public Transform[] spawnPoints; //SpwanPoint;

    public float damageMax = 40f;
    public float damageMin = 10f;

    public float healthMax = 200f;
    public float healthmin = 50f;

    public float SpeedMax = 3f;
    public float SpeedMin = 1f;

    public Material StrongMonsterColor;

    public List<Monster> monsters = new List<Monster>(); //CreateMonsterAdd List
    private int Wave; // Round;

    private void Update()
    {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            return;
        }

        // 적을 모두 물리친 경우 다음 스폰 실행
        if (monsters.Count <= 0)
        {
            SpwanWave();
        }
        UpdateUI();
    }
    private void UpdateUI()
    {
        // 현재 웨이브와 남은 적의 수 표시
        UiManager.instance.UpdateWaveText(Wave, monsters.Count);
    }

    private void SpwanWave()
    {
        Wave++; //count Up Wave;

        //now wave * 1.5 Round Up SpwanMonster
        int spwanCount = Mathf.RoundToInt(Wave * 1.5f);

        for(int i=0;i <spwanCount;i++) {
            float enemtIntensity = Random.Range(0f,1f);

            createMonster(enemtIntensity);
        }

    }

    private void createMonster(float enemtIntensity)
    {
        float health = Mathf.Lerp(healthmin, healthMax, enemtIntensity);
        float damage = Mathf.Lerp(damageMin, damageMax, enemtIntensity);
        float speed = Mathf.Lerp(SpeedMin, SpeedMax, enemtIntensity);

        //Create Position is Random
        Transform spwanPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Create Monster
        Monster monster = Instantiate(MonsterPrefab, spwanPoint.position, spwanPoint.rotation);

        //setting info
        monster.SetUp(health, damage, speed);

        //listadd
        monsters.Add(monster);

        //Evnent
        monster.OnDeath += () => monsters.Remove(monster);
        monster.OnDeath += () => Destroy(monster.gameObject, 5f);
        monster.OnDeath += () => GameManager.Instance.AddScore(100);

    }
}
