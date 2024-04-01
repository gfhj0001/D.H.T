using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repostion : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 Playerpos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(Playerpos.x - myPos.x);
        float diffY = Mathf.Abs(Playerpos.y - myPos.y);

        Vector3 platerDir = GameManager.instance.player.inputVec;
        float dirX = platerDir.x < 0 ? -1 : 1;
        float dirY = platerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                // 타일맵 하나의 크기는 20 -> 두 칸 건너뛰어야 하니 40
                if (diffX > diffY) {
                    transform.Translate(dirX * 40, 0, 0);
                } else if (diffX < diffY) {
                    transform.Translate(0, dirY * 40, 0);
                } else {
                    transform.Translate(dirX * 40, dirY * 40, 0);
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(platerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }

    }
}
