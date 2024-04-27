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

    public const float BOSS_SPAWN_TIME = 5.0f; // 예를 들어, 60초 후에 보스 몬스터를 스폰하려면 이 값을 60.0f로 설정합니다.
    public const int BOSS_HEALTH = 1000; // 보스의 체력을 1000으로 설정
    public const float BOSS_SPEED = 5.0f; // 보스의 이동속도를 5.0으로 설정
    private bool bossSpawned = false;

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

        // 보스 몬스터 스폰 로직
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
            boss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

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