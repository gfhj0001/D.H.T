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
            case 92:
                Damageup();
                break;
            case 93:
                Healthup();
                break;
            case 94:
                Takedamagedown();
                break;
            case 95:
                Recovery();
                break;
            case 96:
                Lifesteal();
                break;
            case 97:
                RateUp();
                break;
            case 98:
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
        GameManager.instance.maxHealth *= (1 + rate);
        GameManager.instance.health = GameManager.instance.maxHealth; //일단 최대체력이 증가하면서 전체 체력도 회복하는 방식으로 코드 작성
    }
    void Takedamagedown()
    {
        //switch나 flag를 통해 count값을 player스크립트의 55줄로 갖고 가기
    }
    void Recovery()
    {
         
    }
    void Lifesteal()
    {
        //enemy 75줄
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
            }
        }
    }

    void SpeedUp() //이동속도
    {
        float speed = 5;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
