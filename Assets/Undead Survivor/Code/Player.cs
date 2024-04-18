using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;

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
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
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
        
        anim.SetFloat("Speed", inputVec.magnitude); 

        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
        return;

        GameManager.instance.health -= Time.deltaTime * 10; //프레임마다 이벤트가 실행되기 때문에 적절하게 체력이 깎이게 하기 위해 deltaTime을 이용

        if (GameManager.instance.health < 0){//체력이 0보다 작아지면 사망
            for (int index=2; index < transform.childCount; index++){
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
