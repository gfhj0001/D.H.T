using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    public Text textBox;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
        textBox = GameObject.Find("Text Title").GetComponent<Text>();
    }

    public void Show()
    {
        
        Next();
        rect.localScale = Vector3.one;
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

    public void Select()
    {

        textBox.text = "무기를 선택해주세요.";

        rect.localScale = Vector3.one;
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

    void Next() {
        textBox.text = "레벨이 상승했습니다.";

        // 1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 활성화할 아이템 리스트
        List<Item> activeItems = new List<Item>();
        ItemData[] weaponData = new ItemData[3];
        ItemData[] gemData = new ItemData[3];

        if (GameManager.instance.itemWeapons.Count > 0) {
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


        // 3. 만렙이 아닌 아이템을 활성화할 리스트에 추가
        int selected = 0;
        Debug.Log("==============================================");
        if (GameManager.instance.itemGems.Count == 3 && GameManager.instance.itemWeapons.Count == 3) { //무기와 보석을 이미 3개씩 선택 했을때
            Debug.Log("무기와 보석을 이미 3개씩 선택 했을때");
            foreach (Item item in items) {
                if (item.data.itmeId == 99) {
                    activeItems.Add(item);
                }
                for(int i = 0; i < 3; i++) {
                    if (item.data == weaponData[i] && item.level < item.data.damages.Length) {
                        Debug.Log("무기만 3개를 선택했을 때 무기가 추가됨.");
                        activeItems.Add(item);
                    }
                    if (item.data == gemData[i] && item.level < item.data.damages.Length) {
                        activeItems.Add(item);
                    }
                }
            }
            
        } else if (GameManager.instance.itemWeapons.Count == 3) { //무기만 3개를 선택했을 때
            Debug.Log("무기만 3개를 선택했을 때");
            foreach (Item item in items) {
                for(int i = 0; i < GameManager.instance.itemWeapons.Count; i++) {
                    if (item.data == weaponData[i] && item.level < item.data.damages.Length) {
                        Debug.Log("무기만 3개를 선택했을 때 무기가 추가됨.");
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
                        Debug.Log($"{item}, 추가된 보석");
                        break;
                    } else if (item.data == gemData[i] && item.level == item.data.damages.Length) {
                        Debug.Log("보석이 만렙이라 추가되지 않았음");
                        break;
                    }
                }
                if (item.level < item.data.damages.Length && selected == 0) {
                    if(item.data.itemType == ItemData.ItemType.Gem) {
                        activeItems.Add(item);
                        Debug.Log($" {item}, 선택한적 없는 보석이 추가됨.");
                    }
                }
            }
        } else if (GameManager.instance.itemGems.Count == 3) { //보석만 3개를 선택했을때
            Debug.Log("보석만 3개를 선택했을때");
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
                        Debug.Log($"{item}, 추가된 무기");
                        break;
                    } else if (item.data == weaponData[i] && item.level == item.data.damages.Length) {
                        Debug.Log("무기가 만렙이라 추가되지 않았음");
                        break;
                    }
                }
                // 선택한적 없는 무기를 추가하는 로직.
                if (item.level < item.data.damages.Length && selected == 0) {
                    if ((item.data.itemType == ItemData.ItemType.Melee) || (item.data.itemType == ItemData.ItemType.Range)) {
                        activeItems.Add(item);
                        Debug.Log($" {item}, 선택한적 없는 무기가 추가됨.");
                    }
                }
            }
        } else { //선택한 무기와 보석이 각각 3개가 되지 않을때
            Debug.Log("선택한 무기와 보석이 각각 3개가 되지 않을때");
            foreach (Item item in items) {   
            selected = 0;
            //기존에 선택했던 무기를 리스트에 추가하는 로직
                for (int i = 0; i < 3; i++){
                    if(item.data == weaponData[i] && item.level < item.data.damages.Length){
                        activeItems.Add(item);
                        selected = 1;
                        Debug.Log($"{item}, 추가된 무기");
                        break;
                    } else if (item.data == weaponData[i] && item.level == item.data.damages.Length) {
                        Debug.Log("무기가 만렙이라 추가되지 않았음");
                        break;
                    }

                    if (item.data == gemData[i] && item.level < item.data.damages.Length) {
                        activeItems.Add(item);
                        selected = 1;
                        Debug.Log($"{item}, 추가된 보석");
                        break;
                    } else if (item.data == gemData[i] && item.level == item.data.damages.Length) {
                        Debug.Log("보석이 만렙이라 추가되지 않았음");
                        break;
                    }
                }
                // 선택한적 없는 무기를 추가하는 로직.
                if (item.level < item.data.damages.Length && selected == 0) {
                    if ((item.data.itemType == ItemData.ItemType.Melee) || (item.data.itemType == ItemData.ItemType.Range)) {
                        activeItems.Add(item);
                        Debug.Log($" {item}, 선택한적 없는 무기가 추가됨.");
                    }
                    if(item.data.itemType == ItemData.ItemType.Gem) {
                        activeItems.Add(item);
                        Debug.Log($" {item}, 선택한적 없는 보석이 추가됨.");
                    }
                }
            }
        }
        
        Debug.Log("==============================================");

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
            Debug.Log($"{activeItems[i]}, 모두 만렙이라 맥주만 선택가능함.");
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
            Debug.Log("==============================================");
            Debug.Log($"{ran[0]}, 0번째");
            Debug.Log($"{ran[1]}, 1번째");
            Debug.Log($"{ran[2]}, 2번째");

            for (int i = 0; i < 3; i++) {
                activeItems[ran[i]].gameObject.SetActive(true);
                Debug.Log($"{activeItems[ran[i]]}, 출력하는 아이템.");
            }
            Debug.Log("==============================================");    
        }
    }
}
