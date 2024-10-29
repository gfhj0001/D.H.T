using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag) {
            case "Ground":

                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Math.Abs(diffX);
                diffY = Math.Abs(diffY);

                if (Mathf.Abs(diffX - diffY) <= 2.0f)
                {
                this.transform.Translate(Vector3.right * dirX * 40);
                this.transform.Translate(Vector3.up * dirY * 40);
                }
                else if (diffX > diffY)
                {
                this.transform.Translate(Vector3.right * dirX * 40); 
                }
                else if (diffX < diffY)
                {
                this.transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
            //몬스터 재배치 로직 사용 안함.
                break;
        }
    }
}