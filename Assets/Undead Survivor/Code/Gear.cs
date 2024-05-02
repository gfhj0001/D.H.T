using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;
    public int id;

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itmeId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        id = data.itmeId;
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

   void ApplyGear()
    {
        switch(id) {
            case 92: //공격력 증가
                Damageup();
                break;
            case 93: //체력 증가
                Healthup();
                break;
            case 94: //받피감 증가
                Takedamagedown();
                break;
            case 95: //3초마다 체력 회복
                Hp_regeneration();
                break;
            case 96: //생명력 흡수
                Lifesteal();
                break;
            case 97: //공속 증가
                RateUp();
                break;
            case 98: //이속 증가
                SpeedUp();
                break;
        }
    }
    void Damageup()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in weapons) {
            weapon.damage *= (1 + rate);
        }
    }
    void Healthup()
    {
        float increasedMaxHealth = 1000 * rate; //게임 시작시 최대 체력에서 증가.
        GameManager.instance.maxHealth += increasedMaxHealth;
        GameManager.instance.health += increasedMaxHealth;
        //최대 체력이 늘어난만큼 현재 체력도 늘어남.
    }
    void Takedamagedown()
    {
        GameManager.instance.takedmgnd = rate;
    }
    void Hp_regeneration()
    {
        if (rate == 1) {
            StartCoroutine(RegenerateHealth(rate));
        } else {
            StopAllCoroutines();
            StartCoroutine(RegenerateHealth(rate));
        }

    }

    IEnumerator RegenerateHealth(float hp_regen_rate)
    {
        float currentHealth;
        while (true) // 무한 반복
        {
            yield return new WaitForSeconds(5f); // 5초 기다림

            currentHealth = GameManager.instance.health;

            // 체력을 rate에 따라 회복함.
            currentHealth += GameManager.instance.maxHealth * hp_regen_rate;

            // 현재 체력이 최대 체력을 초과하지 않도록 제한
            GameManager.instance.health = Mathf.Min(currentHealth, GameManager.instance.maxHealth);
        }
    }

    void Lifesteal()
    {
        GameManager.instance.lifeSteal = rate;
    }

    void RateUp() //연사속도
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons) {
            switch(weapon.id) {
                case 0: //방패
                    weapon.speed_sheild = -150 + (-150 * rate);
                    break;
                case 1: //망치
                    weapon.speed_hammer = 100 + (100 * rate);
                    break;
                case 2 : //창
                    weapon.speed = 0.75f * (1f - rate);
                    break;
                case 3 : //단검
                    weapon.speed_knife = weapon.speed_knife * (1f - rate);
                    break;
                case 4 : //모루
                    weapon.speed_Anvil = weapon.speed_Anvil * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp() //이동속도
    {
        float speed = 5;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
