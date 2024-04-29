using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    // public float levelTime;
    int level;
    float timer;

    public const float BOSS_SPAWN_TIME = 5.0f; // 5초 후 보스가 스폰됨. 
    public const int BOSS_HEALTH = 1000; // 보스 체력
    public const float BOSS_SPEED = 5.0f; // 보스 이동 속도
    private bool bossSpawned = false; //보스의 생존 여부

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
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            GameManager.instance.gameLevel = level;
            Spawn();
        }

        // 
        // if (!bossSpawned && GameManager.instance.gameTime >= BOSS_SPAWN_TIME)
        // {
        //     SpawnBoss();
        //     bossSpawned = true; // 
        // }
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

            // 
            Enemy bossEnemy = boss.GetComponent<Enemy>();
            bossEnemy.health = BOSS_HEALTH;
            bossEnemy.speed = BOSS_SPEED;

            // 
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