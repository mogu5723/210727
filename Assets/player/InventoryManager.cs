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
    Inventory quickslot;
    public Sprite[] itemSprite;
    int invenSize;
    private void Awake() {
        invenSize = 20;
        inven = new List<Inventory>();

        quickslot = new Inventory();
        quickslot.imageObj = transform.Find("quickslot1").gameObject;
        quickslot.bgImage = quickslot.imageObj.transform.Find("bgImage").gameObject.GetComponent<Image>();
        quickslot.image = quickslot.imageObj.transform.Find("image").gameObject.GetComponent<Image>();
        quickslot.textGUI = quickslot.imageObj.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        quickslot.count = 0;
        quickslot.image.enabled = false;
        quickslot.textGUI.enabled = false;

        inven.Add(quickslot);

        for(int i = 0; i < invenSize; i++) inven.Add(createEmptyInven());
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
