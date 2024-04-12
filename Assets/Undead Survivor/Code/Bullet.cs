using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) 
    {
        this.damage = damage;
        this.per = per;

        if(per > -1 ) { //관통이 -1 보다 큰것에 대해서는 속도를 적용함.
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if (!collision.CompareTag("Enemy") || per == -1) //-1은 근접무기 이므로 관통에 대해 신경쓸필요 없음.
            return; 

        per--;

        if (per == -1) {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

}
