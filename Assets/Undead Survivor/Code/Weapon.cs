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
    public int level;
    public float damage;
    public int count;
    public float speed;
    public float speed_knife;
    public float speed_sheild;
    public float speed_hammer;
    public float speed_whip;

    public float speed_Anvil; //모루
    public GameObject lavaAreaPrefab; // 용암 영역 프리팹
    public float speed_LavaBucket;

    GameObject leftwhip;
    GameObject rightwhip;
    
    Weapon wpn;
    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
         leftwhip = player.transform.Find("leftwhip").gameObject;
        rightwhip = player.transform.Find("rightwhip").gameObject;
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
            case 4: // 모루
                timer += Time.deltaTime;

                if (timer > speed_Anvil)
                {
                    timer = 0f;
                    Fire_Anvil();
                }
                break;
            case 5: // 채찍

                timer += Time.deltaTime;

                if (timer > speed_whip)
                {
                   timer = 0f;
                   Batch_whip();
                }
                break;
            case 6: // 용암 
                timer += Time.deltaTime;

                if (timer > speed_LavaBucket)
                {
                    timer = 0f;
                    Fire_LavaBucket();
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
                // player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
                break;
            case 1: 
                // Batch_hammer();
                // player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            case 4: // 모루 [레벨업시 공속증가 X]
                break;
            case 5:
                switch (count) { //채찍 공속 증가
                    case 1:
                        speed_whip = speed_whip * (1f - 0.1f);
                        break;
                    case 2:
                        speed_whip = speed_whip * (1f - 0.15f);
                        break;
                    case 3:
                        speed_whip = speed_whip * (1f - 0.2f);
                        break;
                    case 4:
                        speed_whip = speed_whip * (1f - 0.25f);
                        break;
                    default:
                        speed_whip = speed_whip * (1f - 0.35f);
                        break;
                }
            break;
            case 6: // 용암
                break;

        }
        level++;
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
        level = 1;
        
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
                speed_hammer = 200;
                GameManager.instance.hammerWaitingTime = 2.5f;
                Batch_hammer();
                GameManager.instance.StartHammerCorutine();
                break;
            case 2: //창
                speed = 0.75f; // 1.5초에 2발 발사.
                break;
            case 3: //단검 랜덤한 방향으로 날아감.
                speed_knife = 0.6f; // 1.2초에 2발 발사.
                break;
            case 4: // 모루
                speed_Anvil = 1f; 
                break;
            case 5: // 채찍
                speed_whip = 2f; 
                Batch_whip();
                break;
            case 6: // 용암 양동이
                lavaAreaPrefab = data.projectile; // 용암 영역 프리팹 가져오기
                speed_LavaBucket = 1f; // 1초에 1번 용암 양동이 투척
                break;
        }

        // player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

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
            bullet.Translate(bullet.up * 1, Space.World);
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
    void Fire_Anvil() //모루의 공격방식 
    {
        if (!player.scanner.nearestTarget)
            return;

        // 목표 방향 계산
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dirToTarget = (targetPos - transform.position).normalized;

        // 무작위한 방향 벡터 생성
        Vector3 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

        // 위쪽 방향 설정
        Vector3 upDir = Vector3.up;

        // 위쪽 방향 벡터에 무작위성 벡터를 더함.
        upDir += randomDirection;

        // 위쪽 방향과 적을 향한 방향 사이의 중간 방향 계산
        Vector3 dir = Vector3.Lerp(upDir, dirToTarget, 0.5f).normalized;

        Transform Anvil = GameManager.instance.Pool.Get(prefabId).transform;
        Anvil.position = transform.position;
        Anvil.rotation = Quaternion.FromToRotation(Vector3.up, dir); // 방향을 중간 방향으로 설정
        Anvil.GetComponent<Bullet>().Init(damage, count, dir); // 스틸볼에게 방향 설정

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Ranege);
    }
    // IEnumerator ShowWhip()
    // {
    // leftwhip.SetActive(true);
    // rightwhip.SetActive(true);
    
    // // 0.5초 동안 대기
    // yield return new WaitForSeconds(0.5f);
    
    // leftwhip.SetActive(false);
    // rightwhip.SetActive(false);
    // }

void Batch_whip()
{
    for (int index = 0; index < count; index++)
    {
        Transform bullet;

        if (index < transform.childCount)
        {
            bullet = transform.GetChild(index);
        }
        else
        {
            bullet = GameManager.instance.Pool.Get(prefabId).transform;
            bullet.parent = transform;
        }

        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.identity;

        Vector3 rotVec = Vector3.forward * (360 * index / count + 90);
        bullet.Rotate(rotVec);
        bullet.Translate(bullet.up * 1.5f, Space.World);
        bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);

        // 비활성화 코루틴 시작, 해당 총알의 Weapon을 전달
        StartCoroutine(whipdelay(bullet.gameObject));
    }
}

    void Fire_LavaBucket()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;

        // 적 위치에 용암 생성 로직
        GameObject lavaArea = Instantiate(lavaAreaPrefab, targetPos, Quaternion.identity);
        DamageOverTimeArea dotArea = lavaArea.GetComponent<DamageOverTimeArea>();
      

        // 5초 후에 용암 제거
        Destroy(lavaArea, 5f);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Ranege);
    }
    IEnumerator whipdelay(GameObject bulletObject)
    {
        // 해당 총알 활성화
        bulletObject.SetActive(true);

        // 0.5초 대기 후 비활성화
        yield return new WaitForSeconds(0.5f);
        bulletObject.SetActive(false);
    }
}