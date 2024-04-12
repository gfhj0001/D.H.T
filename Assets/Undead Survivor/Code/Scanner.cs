using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets; //다수를 검색하므로 배열임.
    public Transform nearestTarget;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        // 캐스팅 시작 위치 / 원의 반지름 / 캐스팅 방향 / 캐스팅 길이 / 대상 레이어
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets){
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff) {
                diff = curDiff;
                result = target.transform;
            } 
        } //foreach문을 빠져나오면 가장 가까이에 있는 타겟이 result에 저장됨.

        return result;
    }


}
