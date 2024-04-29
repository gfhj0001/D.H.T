using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

     // 보스 전용 프리팹 인덱스
 public const int BOSS_PREFAB_INDEX = 1; // 보스 전용 프리팹을 2 번째 위치에 두었다고 가정
 public const int MID_BOSS_PREFAB_INDEX = 6; // 중간 보스 전용 프리팹을 6 번째 위치에 두었다고 가정
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}