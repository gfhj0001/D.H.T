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

    public const float BOSS_SPAWN_TIME = 5.0f; // ���� ���, 60�� �Ŀ� ���� ���͸� �����Ϸ��� �� ���� 60.0f�� �����մϴ�.
    public const int BOSS_HEALTH = 1000; // ������ ü���� 1000���� ����
    public const float BOSS_SPEED = 5.0f; // ������ �̵��ӵ��� 5.0���� ����
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

        // ���� ���� ���� ����
        if (!bossSpawned && GameManager.instance.gameTime >= BOSS_SPAWN_TIME)
        {
            SpawnBoss();
            bossSpawned = true; // ������ �����Ǿ����� ǥ��
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

            // ������ �Ӽ��� ���� ����
            Enemy bossEnemy = boss.GetComponent<Enemy>();
            bossEnemy.health = BOSS_HEALTH;
            bossEnemy.speed = BOSS_SPEED;

            // �ʿ��� ��� �߰����� ���� ���� �Ӽ��� ���⿡�� ������ �� �ֽ��ϴ�.
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