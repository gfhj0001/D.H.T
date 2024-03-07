using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager Pool;
    public Player player;
    private Transform target; // 프리펩에서 타겟 설정이 안되어 타겟전용 변수 추가

    void Awake()
    {
        instance = this;
    }

    // 타겟 설정 메서드
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // 타겟 반환 메서드
    public Transform GetTarget()
    {
        return target;
    }
}
