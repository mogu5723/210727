using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory
{
    public GameObject imageObj;
    public Image bgImage, image, cooldownMask;
    public Text textGUI;
    public int itemCode, count, maxCount;
    public float cooldown;
    public string itemTag;
}
public class InventoryManager : MonoBehaviour
{
    public DataManagement dataManagement;
    public List<Inventory> inven;
    public Sprite[] itemSprite;
    //int invenSize;

    private void Awake() {
        //invenSize = 20;
        inven = new List<Inventory>();

        //for(int i = 0; i < invenSize; i++) inven.Add(createEmptyInven());
        for (int i = 0; i < 5; i++) inven.Add(new Inventory());
        for (int i = 1; i <= 5; i++){
            inven[i - 1].imageObj = transform.Find("quickslot" + i).gameObject;
            inven[i - 1].bgImage = inven[i - 1].imageObj.transform.Find("bgImage").gameObject.GetComponent<Image>();
            inven[i - 1].image = inven[i - 1].imageObj.transform.Find("image").gameObject.GetComponent<Image>();
            inven[i - 1].cooldownMask = inven[i - 1].imageObj.transform.Find("cooldownMask").gameObject.GetComponent<Image>();
            inven[i - 1].textGUI = inven[i - 1].imageObj.transform.Find("Text").gameObject.GetComponent<Text>();
            inven[i - 1].count = 0;
            inven[i - 1].cooldown = 0;
            inven[i - 1].image.enabled = false;
            inven[i - 1].textGUI.enabled = false;
        }
    }

    private void Start() {
        addItem(0, "projectile", 30, 99);
    }

    void Update()
    {
        
    }


    public void addItem(int itemCode, string tag, int count, int maxCount){
        foreach(Inventory space in inven){
            if(space.count > 0 && space.count < space.maxCount && space.itemCode == itemCode){
                space.count += count;
                if(space.count > maxCount){
                    count = space.count - maxCount;
                    space.count = maxCount;
                }else count = 0;

                space.textGUI.text = ""+space.count;
                if(count == 0) return;
            } 
        }
        foreach(Inventory space in inven){
            if(space.count == 0){
                space.count += count;
                if(space.count > maxCount){
                    count = space.count - maxCount;
                    space.count = maxCount;
                }else count = 0;

                space.itemCode = itemCode;
                space.itemTag = tag;
                space.maxCount = maxCount;

                space.image.sprite = itemSprite[itemCode]; space.image.enabled = true;
                space.textGUI.text = ""+space.count; space.textGUI.enabled = true;

                if(count == 0) return;
            } 
        }
    }
    public void deleteItem(int slotNumber, int count){
        if(inven[slotNumber].count < count) return;

        inven[slotNumber].count -= count;
        inven[slotNumber].textGUI.text = inven[slotNumber].count+"";

        if(inven[slotNumber].count == 0){
            inven[slotNumber].itemTag = "";
            inven[slotNumber].image.enabled = false;
            inven[slotNumber].textGUI.enabled = false;
            inven[slotNumber].cooldownMask.fillAmount = 0;
        } 
    }
    public IEnumerator cooldown(string tag, float time){
        float cooltime = time;

        foreach(Inventory slot in inven){
            if(slot.itemTag == tag){
                slot.cooldown = time;
                slot.cooldownMask.fillAmount = 1f;
            }
        }

        while(time > 0){
            yield return null;
            time -= Time.deltaTime;
            foreach(Inventory slot in inven){
                if(slot.itemTag == tag){
                    slot.cooldown = time;
                    slot.cooldownMask.fillAmount = time/cooltime;
                }
            }
        }
    }

    public void invenLoad(){
        int invenCount = dataManagement.gameData.invenCount;
        int i;
        for(i = 5; i < invenCount; i++) inven.Add(new Inventory()); //퀵슬롯 외의 슬롯 미구현
        
        i = 0;
        foreach(Inventory slot in inven){
            slot.itemCode = dataManagement.gameData.invenItemCode[i];
            slot.count = dataManagement.gameData.invenItemCount[i++];

            if(slot.count > 0){
                slot.image.sprite = itemSprite[slot.itemCode]; slot.image.enabled = true;
                slot.textGUI.text = "" + slot.count; slot.textGUI.enabled = true;
            }else{
                slot.image.enabled = false;
                slot.textGUI.enabled = false;
            }
        }
    }
    Inventory createEmptyInven(){
        Inventory newInven = new Inventory();
        newInven.count = 0;
        return newInven;
    }
}
