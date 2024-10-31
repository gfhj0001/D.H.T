using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    int level;
    float timer;

    public const float BOSS_SPAWN_TIME = 45.0f; // n초 후 보스가 스폰됨. 
    public const int BOSS_HEALTH = 375; // 보스 체력
    public const float BOSS_SPEED = 4.0f; // 보스 이동 속도
    private bool bossSpawned = false; //보스의 생존 여부


    private 
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        // levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 60f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            GameManager.instance.gameLevel = level;
            Spawn();
        }
   
        
        if (!bossSpawned && GameManager.instance.gameTime >= BOSS_SPAWN_TIME)
        {
            SpawnBoss();
            bossSpawned = true; // 보스가 스폰되었음을 표시
        }
    }

        void Spawn()
        {
            GameObject enemy = GameManager.instance.Pool.Get(0);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }

    void SpawnBoss()
        {
            GameObject boss = GameManager.instance.Pool.Get(PoolManager.BOSS_PREFAB_INDEX);
            boss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; //지정해서 하면 좋을ㅡㄷㅅ?

        
            // 보스의 속성을 직접 설정
            Enemy bossEnemy = boss.GetComponent<Enemy>();
            bossEnemy.health = BOSS_HEALTH;
            bossEnemy.speed = BOSS_SPEED;

             
            // 필요한 경우 추가적인 보스 전용 속성을 여기에서 설정할 수 있습니다.
        }
    }
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}