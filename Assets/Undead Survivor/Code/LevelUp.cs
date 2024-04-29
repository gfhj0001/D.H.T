using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class LevelUp : MonoBehaviour
{
    Item[] items;
    RectTransform rect;
    RectTransform weaponRect;
    RectTransform gemRect;
    public Image[] weaponImage = new Image [3];
    ItemData[] changedWeaponImage = new ItemData[3];

    int num1 = 0;
    int num2 = 0;
    public Image[] gemImage = new Image [3];
    ItemData[] changedGemImage = new ItemData[3];


    RectTransform selectTextBox;
    RectTransform levelTextBox;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        weaponRect = GameObject.Find("Weapon Panel").GetComponent<RectTransform>();
        gemRect = GameObject.Find("Gem Panel").GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
        selectTextBox = GameObject.Find("Select Text Title").GetComponent<RectTransform>();
        levelTextBox = GameObject.Find("Level Text Title").GetComponent<RectTransform>();
        weaponImage[0] = GameObject.Find("Weapon Image 1").GetComponent<Image>();
        weaponImage[1] = GameObject.Find("Weapon Image 2").GetComponent<Image>();
        weaponImage[2] = GameObject.Find("Weapon Image 3").GetComponent<Image>();
        gemImage[0] = GameObject.Find("Gem Image 1").GetComponent<Image>();
        gemImage[1] = GameObject.Find("Gem Image 2").GetComponent<Image>();
        gemImage[2] = GameObject.Find("Gem Image 3").GetComponent<Image>();
    }

    public void Select()
    {
        rect.localScale = Vector3.one;
        levelTextBox.localScale = Vector3.zero;
        weaponRect.localScale = Vector3.zero;
        gemRect.localScale = Vector3.zero;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);

        // 1. 모든 아이템 비활성화
        foreach (Item item in items) {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤하게 3개 아이템만 활성화
        int[] ran = new int[3];
        while (true) {
            ran[0] = Random.Range(0, 4);
            ran[1] = Random.Range(0, 4);
            ran[2] = Random.Range(0, 4);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;    
        }

        for (int index=0; index < ran.Length; index++) {
            Item ranItem = items[ran[index]];
            ranItem.gameObject.SetActive(true);
        }
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        selectTextBox.localScale= Vector3.zero;
        levelTextBox.localScale = Vector3.one;
        weaponRect.localScale = Vector3.one;
        gemRect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    void Next() {
        // 1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 활성화할 아이템 리스트
        List<Item> activeItems = new List<Item>();
        ItemData[] weaponData = new ItemData[3];
        ItemData[] gemData = new ItemData[3];

        if (GameManager.instance.itemWeapons.Count > 0) { //
            for (int i = 0; i < GameManager.instance.itemWeapons.Count; i++) 
            {
            weaponData[i] = GameManager.instance.itemWeapons[i];
            }
        }

        if (GameManager.instance.itemGems.Count > 0) {
            for (int i = 0; i < GameManager.instance.itemGems.Count; i++) 
            {
            gemData[i] = GameManager.instance.itemGems[i];
            }
        }


        for(int index = 0; index < 3; index++) {
            foreach (Item item in items) {
                if (item.data == weaponData[index]) {
                    if(changedWeaponImage[index] == weaponData[index]){
                        break;
                    }
                    ChangeWpnSprite(item);

                }
            }
        }

        for(int index = 0; index < 3; index++) {
            foreach (Item item in items) {
                if (item.data == gemData[index]) {
                    if(changedGemImage[index] == gemData[index]){
                        break;
                    }
                    ChangeGemSprite(item);
                }
            }
        }

        void ChangeWpnSprite(Item item) {

            if (num1 == 0) {
                weaponImage[0].sprite = item.data.itemIcon;
                changedWeaponImage[0] = item.data;
                num1++;
            } else if (num1 == 1) {
                weaponImage[1].sprite = item.data.itemIcon;
                changedWeaponImage[1] = item.data;
                num1++;
            } else if (num1 == 2) {
                weaponImage[2].sprite = item.data.itemIcon;
                changedWeaponImage[2] = item.data;
                num1++;
            }
            
        }

        void ChangeGemSprite(Item item) {

            if (num2 == 0) {
                gemImage[0].sprite = item.data.itemIcon;
                changedGemImage[0] = item.data;
            } else if (num2 == 1) {
                gemImage[1].sprite = item.data.itemIcon;
                changedGemImage[1] = item.data;
            } else if (num2 == 2) {
                gemImage[2].sprite = item.data.itemIcon;
                changedGemImage[2] = item.data;
            }
            num2++;
        }


        // 3. 만렙이 아닌 아이템을 활성화할 리스트에 추가
        int selected = 0;
        if (GameManager.instance.itemGems.Count == 3 && GameManager.instance.itemWeapons.Count == 3) { //무기와 보석을 이미 3개씩 선택 했을때
            foreach (Item item in items) {
                if (item.data.itmeId == 99) {
                    activeItems.Add(item);
                }
                for(int i = 0; i < 3; i++) {
                    if (item.data == weaponData[i] && item.level < item.data.damages.Length) {
                        activeItems.Add(item);
                    }
                    if (item.data == gemData[i] && item.level < item.data.damages.Length) {
                        activeItems.Add(item);
                    }
                }
            }
            
        } else if (GameManager.instance.itemWeapons.Count == 3) { //무기만 3개를 선택했을 때
            foreach (Item item in items) {
                for(int i = 0; i < GameManager.instance.itemWeapons.Count; i++) {
                    if (item.data == weaponData[i] && item.level < item.data.damages.Length) {
                        activeItems.Add(item);
                    }
                }
            }
            foreach (Item item in items) {   
                selected = 0;
                for (int i = 0; i < 3; i++){
                    if (item.data == gemData[i] && item.level < item.data.damages.Length) {
                        activeItems.Add(item);
                        selected = 1;
                        break;
                    } else if (item.data == gemData[i] && item.level == item.data.damages.Length) {
                        break;
                    }
                }
                if (item.level < item.data.damages.Length && selected == 0) {
                    if(item.data.itemType == ItemData.ItemType.Gem) {
                        activeItems.Add(item);
                    }
                }
            }
        } else if (GameManager.instance.itemGems.Count == 3) { //보석만 3개를 선택했을때
            foreach (Item item in items) {
                for(int i = 0; i < GameManager.instance.itemWeapons.Count; i++) {
                    if (item.data == gemData[i] && item.level < item.data.damages.Length) {
                        activeItems.Add(item);
                    }
                }
            }
                foreach (Item item in items) {   
                selected = 0;
                for (int i = 0; i < 3; i++){
                    if(item.data == weaponData[i] && item.level < item.data.damages.Length){
                        activeItems.Add(item);
                        selected = 1;
                        break;
                    } else if (item.data == weaponData[i] && item.level == item.data.damages.Length) {
                        break;
                    }
                }
                // 선택한적 없는 무기를 추가하는 로직.
                if (item.level < item.data.damages.Length && selected == 0) {
                    if ((item.data.itemType == ItemData.ItemType.Melee) || (item.data.itemType == ItemData.ItemType.Range)) {
                        activeItems.Add(item);
                    }
                }
            }
        } else { //선택한 무기와 보석이 각각 3개가 되지 않을때
            foreach (Item item in items) {   
            selected = 0;
            //기존에 선택했던 무기를 리스트에 추가하는 로직
                for (int i = 0; i < 3; i++){
                    if(item.data == weaponData[i] && item.level < item.data.damages.Length){
                        activeItems.Add(item);
                        selected = 1;
                        break;
                    } else if (item.data == weaponData[i] && item.level == item.data.damages.Length) {
                        break;
                    }

                    if (item.data == gemData[i] && item.level < item.data.damages.Length) {
                        activeItems.Add(item);
                        selected = 1;
                        break;
                    } else if (item.data == gemData[i] && item.level == item.data.damages.Length) {
                        break;
                    }
                }
                // 선택한적 없는 무기를 추가하는 로직.
                if (item.level < item.data.damages.Length && selected == 0) {
                    if ((item.data.itemType == ItemData.ItemType.Melee) || (item.data.itemType == ItemData.ItemType.Range)) {
                        activeItems.Add(item);
                    }
                    if(item.data.itemType == ItemData.ItemType.Gem) {
                        activeItems.Add(item);
                    }
                }
            }
        }
        

        // 4. 활성화할 아이템이 3개보다 적을 경우, 맥주 아이템 추가
        while (activeItems.Count < 3)
        {
            foreach (Item item in items)
            {
                if (item.data.itmeId == 99) // 맥주 아이템의 itemId를 확인하여 추가
                {
                    activeItems.Add(item);
                    break;
                }
            }
        }

        if (activeItems[0].data.itmeId == 99 && activeItems[1].data.itmeId == 99 && activeItems[2].data.itmeId == 99) {
            for (int i = 0; i < 3; i++) {
            activeItems[i].gameObject.SetActive(true);
            }
        } else {
            int[] ran = new int[3];
            while (true) {
                ran[0] = Random.Range(0, activeItems.Count);
                ran[1] = Random.Range(0, activeItems.Count);
                ran[2] = Random.Range(0, activeItems.Count);

                if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                    break;
            }

            for (int i = 0; i < 3; i++) {
                activeItems[ran[i]].gameObject.SetActive(true);
            }
        }
    }
}
