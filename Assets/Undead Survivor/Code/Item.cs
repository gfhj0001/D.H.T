using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    public Image icon;
    public Text textLevel;
    public Text textName;
    public Text textDesc;

    float currentDamage;
    int currentCount;
    float currentSpeed;


    void Awake() {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        switch (data.itemType) {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:

                if (GameManager.instance.flagDestroyWeapon == true) {
                    SetDestroyDesc(data);
                    break;
                }

                if (level == 0) {
                    SetFirstDesc(data);
                    textLevel.text = "Lv." + (level + 1);
                    break;
                }

                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                textLevel.text = "Lv." + (level + 1);
                break;
            case ItemData.ItemType.Gem:
                textLevel.text = "Lv." + (level + 1);
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textLevel.text = "Lv." + (level + 1);
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void SetDestroyDesc(ItemData data)
    {
        if (data.itmeId == 0)
        { //방패의 id
            Weapon wpn = GameObject.Find("Weapon 0").GetComponent<Weapon>();
            currentDamage = wpn.damage;
            currentCount = wpn.count;
            textLevel.text = "Lv." + wpn.level;
            textDesc.text = "데미지 : " + currentDamage.ToString() + "\n" + "회전체 : " + currentCount.ToString() + "개";
        }
        else if (data.itmeId == 1)
        { //망치의 id
            Weapon wpn = GameObject.Find("Weapon 1").GetComponent<Weapon>();
            currentDamage = wpn.damage;
            currentSpeed = GameManager.instance.hammerDelay;
            currentSpeed = (float)(Math.Floor((1 / currentSpeed) * 10f) / 10f);
            currentCount = wpn.count;
            textLevel.text = "Lv." + wpn.level;
            textDesc.text = "데미지 : " + currentDamage.ToString() + "\n" + "회전체 : " + currentCount.ToString() + "개" + "공격 속도 : 1초에 " + currentSpeed.ToString() + "번 발사";
        }
        else if (data.itmeId == 2)
        { //창의 id
            Weapon wpn = GameObject.Find("Weapon 2").GetComponent<Weapon>();
            currentDamage = wpn.damage;
            currentSpeed = wpn.speed;
            currentCount = wpn.count;
            currentSpeed = (float)(Math.Floor((1 / currentSpeed) * 10f) / 10f);
            textLevel.text = "Lv." + wpn.level;
            textDesc.text = "데미지 : " + currentDamage.ToString() + "\n" + "공격 속도 : 1초에 " + currentSpeed.ToString() + "번 발사" + "\n" + "관통력 : " + currentCount.ToString();
        }
        else if (data.itmeId == 3)
        { //단검의 id
            Weapon wpn = GameObject.Find("Weapon 3").GetComponent<Weapon>();
            currentDamage = wpn.damage;
            currentSpeed = wpn.speed_knife;
            currentSpeed = (float)(Math.Floor((1 / currentSpeed) * 10f) / 10f);
            textLevel.text = "Lv." + wpn.level;
            textDesc.text = "데미지 : " + currentDamage.ToString() + "\n" + "공격 속도 : 1초에 " + currentSpeed.ToString() + "번 발사" + "\n" + "관통력 : 0";
        }
        else if (data.itmeId == 4)
        { //모루의 id
            Weapon wpn = GameObject.Find("Weapon 4").GetComponent<Weapon>();
            currentDamage = wpn.damage;
            currentSpeed = wpn.speed_Anvil;
            currentSpeed = (float)(Math.Floor((1 / currentSpeed) * 10f) / 10f);
            textLevel.text = "Lv." + wpn.level;
            textDesc.text = "데미지 : " + currentDamage.ToString() + "\n" + "공격 속도 : 1초에 " + currentSpeed.ToString() + "번 던짐" + "\n" + "관통력 : 99";
        }
        else if (data.itmeId == 5)
        { //채찍의 id
            Weapon wpn = GameObject.Find("Weapon 5").GetComponent<Weapon>();
            currentDamage = wpn.damage;
            currentSpeed = wpn.speed_whip;
            currentSpeed = (float)(Math.Floor((1 / currentSpeed) * 10f) / 10f);
            textLevel.text = "Lv." + wpn.level;
            textDesc.text = "데미지 : " + currentDamage.ToString() + "\n" + "공격 속도 : 1초에 " + currentSpeed.ToString() + "번 휘두름" + "\n";
        }
        else if (data.itmeId == 6)
        { //용암의 id
            Weapon wpn = GameObject.Find("Weapon 6").GetComponent<Weapon>();
            currentDamage = wpn.damage;
            currentSpeed = GameManager.instance.lavaDelay;
            currentSpeed = (float)(Math.Floor((1 / currentSpeed) * 10f) / 10f);
            textLevel.text = "Lv." + wpn.level;
            textDesc.text = "데미지 : " + currentDamage.ToString() + "\n" + "공격 속도 : 1초에 " + currentSpeed.ToString() + "번 뿌림";
        }
    }


    public void SetFirstDesc(ItemData data)
{
    if (data.itmeId == 0)
        { //방패의 id
            int num = data.baseCount;
            textDesc.text = "주위를 회전하는 방패 " + num.ToString() + "개 소환";
        }
        else if (data.itmeId == 1)
        { //망치의 id
            textDesc.text = "나선형으로 회전하는" + "\n" + "망치 1개 소환";
        }
        else if (data.itmeId == 2)
        { //창의 id
            textDesc.text = "적을 관통하는 창을 투척";
        }
        else if (data.itmeId == 3)
        { //단검의 id
            textDesc.text = "가까운적 주변으로" + "\n" + "단검을 빠르게 투척";
        }
        else if (data.itmeId == 4)
        { //모루의 id
            textDesc.text = "모루 1개 투척";
        }
        else if (data.itmeId == 5)
        { //채찍의 id
            textDesc.text = "적을 관통하는 채찍을 휘두름";
        }
        else if (data.itmeId == 6)
        { //용암의 id
            textDesc.text = "바닥에 용암을 뿌림";
        }
}

    public void OnClick()
    {
        if (GameManager.instance.flagDestroyWeapon == true) {
            OnClickedButtonDstWpn ();
        } else {
            OnClickedButtonLvUp ();
        }
    }

    void OnClickedButtonLvUp () {
        switch(data.itemType) {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0) {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                    GameManager.instance.itemWeapons.Add(data);
                } 
                else {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;
            case ItemData.ItemType.Gem:
                if (level == 0) {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                    GameManager.instance.itemGems.Add(data);
                }
                else {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;        
        }


        if (level == data.damages.Length) {
            GetComponent<Button>().interactable = false;
        }
    }

    void OnClickedButtonDstWpn () {
        RectTransform rectDestroyPanel = GameObject.Find("Destroy Panel").GetComponent<RectTransform>();;
        Weapon wpn;
        Bullet[] blt;
        switch (data.itmeId) {
            case 0: // 방패
                wpn = GameObject.Find("Weapon 0").GetComponent<Weapon>();
                blt = GameObject.Find("Weapon 0").GetComponentsInChildren<Bullet>(true);
                foreach (Bullet b in blt) {
                    b.gameObject.SetActive(false);
                    Destroy(b);
                }
                wpn.gameObject.SetActive(false);
                Destroy(wpn);
                rectDestroyPanel.gameObject.SetActive(false);
                GameManager.instance.itemWeapons.Remove(data);
                GameManager.instance.flagDestroyWeapon = true;
                GameManager.instance.destroyWeapon = data;
                break;
            case 1: // 망치
                GameManager.instance.StopHammerCorutine(); //코루틴 정지
                wpn = GameObject.Find("Weapon 1").GetComponent<Weapon>();
                blt = GameObject.Find("Weapon 1").GetComponentsInChildren<Bullet>(true);
                foreach (Bullet b in blt) {
                    b.gameObject.SetActive(false);
                    Destroy(b);
                }
                wpn.gameObject.SetActive(false);
                Destroy(wpn);
                rectDestroyPanel.gameObject.SetActive(false);
                GameManager.instance.itemWeapons.Remove(data);
                GameManager.instance.flagDestroyWeapon = true;
                GameManager.instance.destroyWeapon = data;
                break;
            case 2: // 창
                wpn = GameObject.Find("Weapon 2").GetComponent<Weapon>();
                blt = GameObject.Find("Weapon 2").GetComponentsInChildren<Bullet>(true);
                foreach (Bullet b in blt) {
                    b.gameObject.SetActive(false);
                    Destroy(b);
                }
                wpn.gameObject.SetActive(false);
                Destroy(wpn);
                rectDestroyPanel.gameObject.SetActive(false);
                GameManager.instance.itemWeapons.Remove(data);
                GameManager.instance.flagDestroyWeapon = true;
                GameManager.instance.destroyWeapon = data;
                break;
            case 3: // 단검
                wpn = GameObject.Find("Weapon 3").GetComponent<Weapon>();
                blt = GameObject.Find("Weapon 3").GetComponentsInChildren<Bullet>(true);
                foreach (Bullet b in blt) {
                    b.gameObject.SetActive(false);
                    Destroy(b);
                }
                wpn.gameObject.SetActive(false);
                Destroy(wpn);
                rectDestroyPanel.gameObject.SetActive(false);
                GameManager.instance.itemWeapons.Remove(data);
                GameManager.instance.flagDestroyWeapon = true;
                GameManager.instance.destroyWeapon = data;
                break;
            case 4: // 모루
                wpn = GameObject.Find("Weapon 4").GetComponent<Weapon>();
                blt = GameObject.Find("Weapon 4").GetComponentsInChildren<Bullet>(true);
                foreach (Bullet b in blt)
                {
                    b.gameObject.SetActive(false);
                    Destroy(b);
                }
                wpn.gameObject.SetActive(false);
                Destroy(wpn);
                rectDestroyPanel.gameObject.SetActive(false);
                GameManager.instance.itemWeapons.Remove(data);
                GameManager.instance.flagDestroyWeapon = true;
                GameManager.instance.destroyWeapon = data;
                break;
            case 5: // 채찍
                wpn = GameObject.Find("Weapon 5").GetComponent<Weapon>();
                blt = GameObject.Find("Weapon 5").GetComponentsInChildren<Bullet>(true);
                foreach (Bullet b in blt)
                {
                    b.gameObject.SetActive(false);
                    Destroy(b);
                }
                wpn.gameObject.SetActive(false);
                Destroy(wpn);
                rectDestroyPanel.gameObject.SetActive(false);
                GameManager.instance.itemWeapons.Remove(data);
                GameManager.instance.flagDestroyWeapon = true;
                GameManager.instance.destroyWeapon = data;
                break;
            case 6: // 용암
                wpn = GameObject.Find("Weapon 6").GetComponent<Weapon>();
                wpn.gameObject.SetActive(false);
                Destroy(wpn);
                rectDestroyPanel.gameObject.SetActive(false);
                GameManager.instance.itemWeapons.Remove(data);
                GameManager.instance.flagDestroyWeapon = true;
                GameManager.instance.destroyWeapon = data;
                break;

        }
    }
}