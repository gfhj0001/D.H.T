using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Transform uiJoy;

    [Header("# ETC..")]
    public float hammerDelay;
    public int flagDestroyWeapon = 0; //무기 파괴 플래그 0 = 파괴X / 1 = 무기 파괴 화면 진입 상태. / -99 = 이미 무기를 파괴한 상태
    public float lavaDamage;
    public float lavaDelay;
    public float whipDelay;
    public Vector3 playerpos;

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
        player.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        //용암 양동이 프리셋이 Player 오브젝트 밖에 있기 때문에 게임 오버시 비활성화 해줘야 함.
        Bullet lavaBuckitBullet = GameObject.Find("Bullet 6(Clone)").GetComponent<Bullet>();
        lavaBuckitBullet.gameObject.SetActive(false);
        
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

        playerpos = player.transform.position;
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
        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }

    public void StartHammerCorutine()
    {
        StartCoroutine("Hammer_Active"); //망치가 나타났다가 사라지는 코루틴
    }

    public void StopHammerCorutine()
    {
        StopCoroutine("Hammer_Active");
    }

    public void StartLavaBuckitCorutine()
    {
        StartCoroutine("LavaBuckit_Active");
    }

    public void StopLavaBuckitCorutine()
    {
        StopCoroutine("LavaBuckit_Active");
    }

    public void StartWhipCorutine()
    {
        StartCoroutine("SetWhipPos");
    }

    public void StopWhipCorutine()
    {
        StopCoroutine("SetWhipPos");
        StopCoroutine("WhipDelay");
    }
    


    IEnumerator Hammer_Active()
    {
        Weapon hammer = GameObject.Find("Weapon 1").GetComponent<Weapon>();
        Bullet hammerBullet = hammer.GetComponentInChildren<Bullet>();
        Transform bullet = hammerBullet.transform;
        hammer.gameObject.SetActive(true);
        while (true) {
            hammerBullet.gameObject.SetActive(true);
            bullet.localPosition = Vector3.zero;
            bullet.Translate(bullet.up * 1, Space.World);
            for(int index = 1; index < 200; index++){
                bullet.Translate(bullet.up * 0.0005f * index , Space.World);
                yield return new WaitForSeconds(0.03f);
            }
            hammerBullet.gameObject.SetActive(false);
            yield return new WaitForSeconds(hammerDelay);
            //3초동안 망치 비활성화
        }
    }

    IEnumerator LavaBuckit_Active()
    {
        Bullet lavaBuckitBullet = GameObject.Find("Bullet 6(Clone)").GetComponent<Bullet>();
        Transform bullet = lavaBuckitBullet.transform;
        float x;
        float y;
        Vector3 pos = Vector3.zero;
        Vector3 tmp;
        while(true)
        {   
            x = UnityEngine.Random.Range(-3f, 3f);
            y = UnityEngine.Random.Range(-8f, 8f);
            pos = new Vector3(x, y, 0f);
            Transform playertransform = GameObject.Find("Player").transform;
            tmp = playertransform.position + pos;
            bullet.position = tmp;
            lavaBuckitBullet.gameObject.SetActive(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.lava);
            
            yield return new WaitForSeconds(5f);
            lavaBuckitBullet = GameObject.Find("Bullet 6(Clone)").GetComponent<Bullet>();//lavaBuckit.GetComponentInChildren<Bullet>();

            lavaBuckitBullet.gameObject.SetActive(false);
            yield return new WaitForSeconds(GameManager.instance.lavaDelay);
        }

    }

    IEnumerator SetWhipPos()
    {
        Bullet[] whipBullet = GameObject.Find("Weapon 5").GetComponentsInChildren<Bullet>();
        Transform[] bullets = { whipBullet[0].transform, whipBullet[1].transform };
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        
        bullets[0].gameObject.SetActive(true);
        bullets[1].gameObject.SetActive(true);
        StartCoroutine("WhipDelay");

        while(true)
        {
            Transform playertransform = GameObject.Find("Player").transform;

            Vector3 addX = new Vector3 (1f, 0f, 0f);

            //채찍의 위치를 플레이어로 옮긴 후, x축으로 1만큼 간격을 줌.
            bullets[1].position = playertransform.position + addX;
            bullets[0].position = playertransform.position + addX * -1f;

            yield return wait; //1프레임 기다림.
        }
    }

    IEnumerator WhipDelay()
    {
        Bullet[] whipBullet = GameObject.Find("Weapon 5").GetComponentsInChildren<Bullet>();
        while (true)
        {
            whipBullet[0].gameObject.SetActive(true);
            whipBullet[1].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f); //애니메이션 시간
            whipBullet[0].gameObject.SetActive(false);
            whipBullet[1].gameObject.SetActive(false);
            yield return new WaitForSeconds(whipDelay); //공격 대기 시간
        }
    }

}
