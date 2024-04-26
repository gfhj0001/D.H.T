using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    public float speed_knife;
    public float speed_sheild;
    public float speed_hammer;
    public float[] array_speed = {};


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
            case 0: //방패
                transform.Rotate(Vector3.back * speed_sheild * Time.deltaTime);
                break;
            case 1: //망치
                transform.Rotate(Vector3.back * speed_hammer * Time.deltaTime);
                break;
            case 2: //창
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
            case 3: //단검
                timer += Time.deltaTime;

                if (timer > speed_knife) {
                    timer = 0f;
                    Fire_knife();
                }
                break;
        }

    }

    public void LevelUp(float damage, int count)
    {

        this.damage = damage;
        this.count += count;



        switch (id) {
            case 0:
                Batch_sheild();
                player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
                break;
            case 1: 
                Batch_hammer();
                player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
                break;
            case 3:
                switch (count) { //단검 레벨업시 공속 증가 로직.
                    case 1:
                        speed_knife = speed_knife * (1f - 0.1f);
                        break;
                    case 2:
                        speed_knife = speed_knife * (1f - 0.15f);
                        break;
                    case 3:
                        speed_knife = speed_knife * (1f - 0.2f);
                        break;
                    case 4:
                        speed_knife = speed_knife * (1f - 0.25f);
                        break;
                    default:
                        speed_knife = speed_knife * (1f - 0.35f);
                        break;
                }
                break;
        }

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
            case 0: //방패
                speed_sheild = -150;
                Batch_sheild();
                break;
            case 1: //망치
                speed_hammer = 100;
                Batch_hammer();
                break;
            case 2: //창
                speed = 0.75f; // 1.5초에 2발 발사.
                break;
            case 3: //단검 랜덤한 방향으로 날아감.
                speed_knife = 0.6f; // 1.2초에 2발 발사.
                break;
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }

    void Batch_sheild()
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
            bullet.Translate(bullet.up * 1.7f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); //-100 is Infinity per
        }
    }

        void Batch_hammer()
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
            bullet.Translate(bullet.up * 3.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); //-100 is Infinity per
        }
    }

    void Fire() //창이 사라지는 범위 화면과 비슷하게 축소 해봐야 될듯?
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

    void Fire_knife() //랜덤한 방향으로 날아감.
    {
        if (!player.scanner.nearestTarget)
            return;

        // 목표 방향 계산
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dirToTarget = (targetPos - transform.position).normalized;

        // 무작위한 방향 벡터 생성
        Vector3 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

        // 목표 방향을 기준으로 무작위 방향을 조정
        Vector3 randomDirAdjusted = randomDirection + dirToTarget;

        // 정규화된 방향 벡터로 변환, 방향벡터는 목표를 향하게하였고 무작위 벡터를 더함으로 단검을 적을 향해 대충 던진듯한 느낌이 나듯이 발사함.
        Vector3 dir = randomDirAdjusted.normalized;

        Transform bullet = GameManager.instance.Pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // 방향을 무작위 방향으로 설정
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // 총알에게 무작위 방향 설정

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Ranege);
    }
}
