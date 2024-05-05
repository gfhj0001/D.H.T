using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxhealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;
    bool isOnLava;

    Rigidbody2D rigid;
    Collider2D coll;
    Collider2D test;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        if (!isLive || (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && !(gameObject.tag == "MidBoss" || gameObject.tag == "Boss")))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxhealth;
    }
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxhealth = data.health;
        health = data.health;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        test = collision;
        float currentHealth = GameManager.instance.health;

        if (!isLive) //충돌한게 Bullet인지 검사함.
        {
            return;
        } else if (collision.CompareTag("Bullet"))
        {
            isOnLava = false;
            health -= collision.GetComponent<Bullet>().damage;
            currentHealth += collision.GetComponent<Bullet>().damage * GameManager.instance.lifeSteal; //가한 피해량의 일정부분 만큼 HP를 회복함.
            // 현재 체력이 최대 체력을 초과하지 않도록 제한
            GameManager.instance.health = Mathf.Min(currentHealth, GameManager.instance.maxHealth);
            StartCoroutine(KnockBack()); //코루틴 호출하는 법 혹은 StartCoroutine("KnockBack");
        } else if (collision.CompareTag("Lava"))
        {
            isOnLava = true;
            StartCoroutine("LavaBuckit");
        } else {
            isOnLava = false;
            return;
        }
            

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            

            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

            // 태그가 "MidBoss"인 몬스터가 죽었을 때 경험치 획득  -- 반지 획득으로 변경예정
            if (gameObject.tag == "MidBoss")
            {
                GameManager.instance.GetExp();
            }

            // 태그가 "Boss"인 몬스터가 죽었을 때만 게임 승리
            if (gameObject.tag == "Boss")
            {
                GameManager.instance.GameVictory();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        isOnLava = false;
    }

    IEnumerator KnockBack() //코루틴함수 생명주기나 비동기로 작동함
    {
        yield return wait; //코루틴의 반환, 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

    }

    IEnumerator LavaBuckit()
    {
        while (isOnLava)
        {
            //생명력 흡수 보석 로직
            float currentHealth = GameManager.instance.health;
            currentHealth += GameManager.instance.lavaDamage * GameManager.instance.lifeSteal;
            GameManager.instance.health = Mathf.Min(currentHealth, GameManager.instance.maxHealth);
            health -= GameManager.instance.lavaDamage;
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
            StartCoroutine(KnockBack()); //넉백
            yield return new WaitForSeconds(0.5f);
        }
    }



    void Dead()
    {
        gameObject.SetActive(false);
    }

}