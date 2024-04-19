using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;


    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }


    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id) {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // .. Test Code..
        if (Input.GetButtonDown("Jump")){
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if(id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itmeId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // property Set
        id = data.itmeId;
        damage = data.baseDamage;
        count = data.baseCount;
        
        for (int index=0; index < GameManager.instance.Pool.prefabs.Length; index++) {
            if (data.projectile == GameManager.instance.Pool.prefabs[index]) {
                prefabId = index;
                break;
            }
        }

        switch (id) {
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                speed = 0.3f; //연사속도라고 생각 1초에 3발씩
                break;
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }

    void Batch()
    {
        for (int index=0; index < count; index++){
            Transform bullet;

            if (index < transform.childCount){ //기존 오브젝트를 활용하고, 모자란것을 풀링해서 가져오기.
                bullet = transform.GetChild(index);
            }
            else{
                bullet = GameManager.instance.Pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
           
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); //-1 is Infinity per
        }
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized; //normalized, 벡터의 방향은 유지하고 크기를 1로 변환, 즉 총알이 나가고자 하는 방향임.


        Transform bullet = GameManager.instance.Pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Ranege);
    }
}
