using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory
{
    public GameObject imageObj;
    public Image bgImage, image;
    public TextMeshProUGUI textGUI;
    public int itemCode, count, maxCount;
}
public class InventoryManager : MonoBehaviour
{
    List<Inventory> inven;
    public Inventory[] quickslot;
    public Sprite[] itemSprite;
    //int invenSize;

    private void Awake() {
        //invenSize = 20;
        inven = new List<Inventory>();
        quickslot = new Inventory[5];

        for(int i = 1; i <= 5; i++){
            quickslot[i-1] = new Inventory();
            quickslot[i-1].imageObj = transform.Find("quickslot"+i).gameObject;
            quickslot[i-1].bgImage = quickslot[i-1].imageObj.transform.Find("bgImage").gameObject.GetComponent<Image>();
            quickslot[i-1].image = quickslot[i-1].imageObj.transform.Find("image").gameObject.GetComponent<Image>();
            quickslot[i-1].textGUI = quickslot[i-1].imageObj.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
            quickslot[i-1].count = 0;
            quickslot[i-1].image.enabled = false;
            quickslot[i-1].textGUI.enabled = false;

            inven.Add(quickslot[i-1]);
        }

        //for(int i = 0; i < invenSize; i++) inven.Add(createEmptyInven());
    }

    void Update()
    {
        
    }


    public void addItem(int itemCode, int count, int maxCount){
        foreach(Inventory space in inven){
            if(space.count > 0 && space.count < space.maxCount && space.itemCode == itemCode){
                space.count++;
                space.textGUI.text = ""+space.count;
                return;
            } 
        }
        foreach(Inventory space in inven){
            if(space.count == 0){
                space.count++;
                space.itemCode = itemCode;
                space.maxCount = maxCount;

                space.image.sprite = itemSprite[itemCode]; space.image.enabled = true;
                space.textGUI.text = ""+space.count; space.textGUI.enabled = true;

                return;
            } 
        }
    }
    Inventory createEmptyInven(){
        Inventory newInven = new Inventory();
        newInven.count = 0;
        return newInven;
    }
}
