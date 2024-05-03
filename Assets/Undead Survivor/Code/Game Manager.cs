using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public int gameLevel;

    [Header("# Player Info")]
    public float health;
    public float maxHealth = 100;
    public List<ItemData> itemWeapons = new List<ItemData>(); //획득한 무기를 저장하는 리스트
    public ItemData destroyWeapon;
    public List<ItemData> itemGems = new List<ItemData>(); //획득한 보석을 저장하는 리스트
    public float takedmgnd; // .. 받는피해감소량 / take damage down ..
    public float lifeSteal;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; //임의로 설정한 값. 추후 수치 조정 필요

    [Header("# Game Object")]
    public PoolManager Pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    [Header("# ETC..")]
    public float hammerWaitingTime;
    public int flagDestroyWeapon = 0; //무기 파괴 플래그

    void Awake()
    {
        instance = this;
    }

    public void GameStart()
    {

        health = maxHealth;
        Stop();
        uiLevelUp.Select(); //임시 스크립트 첫번째 캐릭터 선택

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

      public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
    void Update()
    {
        if (!isLive)
            return;
        
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;
            
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) { // 레벨업 로직
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; //흐르는 시간의 배율, **타임스케일로 배속 가능.**
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void StartHammerCorutine()
    {
        StartCoroutine("Hammer_Active"); //망치가 나타났다가 사라지는 코루틴
    }

    public void StopHammerCorutine()
    {
        StopCoroutine("Hammer_Active");
    }

    IEnumerator Hammer_Active()
    {
        Weapon hammer = GameObject.Find("Weapon 1").GetComponent<Weapon>();
        Bullet hammerBullet = hammer.GetComponentInChildren<Bullet>();
        Transform bullet = hammerBullet.transform;
        hammer.gameObject.SetActive(false);
        while (true) {
            hammer.gameObject.SetActive(true);
            bullet.localPosition = Vector3.zero;
            bullet.Translate(bullet.up * 1, Space.World);
            for(int index = 1; index < 200; index++){
                bullet.Translate(bullet.up * 0.0005f * index , Space.World);
                yield return new WaitForSeconds(0.03f);
            }
            hammer.gameObject.SetActive(false);
            yield return new WaitForSeconds(hammerWaitingTime);
            //3초동안 망치 비활성화
        }
    }

}
