using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager Pool;
    public Player player;
    private Transform target; // �����鿡�� Ÿ�� ������ �ȵǾ� Ÿ������ ���� �߰�

    void Awake()
    {
        instance = this;
    }

    // Ÿ�� ���� �޼���
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Ÿ�� ��ȯ �޼���
    public Transform GetTarget()
    {
        return target;
    }
}
