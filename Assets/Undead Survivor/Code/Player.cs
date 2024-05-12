using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    float takeDamage;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
    }
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    void LateUpdate()
    {

        if (!GameManager.instance.isLive)
            return;
        

        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
        return;

        takeDamage = (10 + GameManager.instance.gameLevel); //기본 피격시 데미지는 10. 프레임마다 이벤트가 실행되기 때문에 적절하게 체력이 깎이게 하기 위해 deltaTime을 이용
        takeDamage -= takeDamage * GameManager.instance.takedmgnd; //Gear 스크립트에서 rate값을 그대로 받아서 사용함.
        GameManager.instance.health -= Time.deltaTime * takeDamage;


        // GameManager.instance.health -= Time.deltaTime * 10; //프레임마다 이벤트가 실행되기 때문에 적절하게 체력이 깎이게 하기 위해 deltaTime을 이용

        if (GameManager.instance.health < 0){//체력이 0보다 작아지면 사망
            for (int index=2; index < transform.childCount; index++){
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
