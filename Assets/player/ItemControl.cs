using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ItemControl : MonoBehaviour
{
    PlayerState state;
    PlayerControl pCtrl;

    public InventoryManager invenManager;

    public GameObject[] itemObjList;
    public GameObject globalLight; public GameObject effectSystem;

    public float projectileCooldown, leftProjectileCD;

    private void Awake() {
        state = GetComponent<PlayerState>();
        pCtrl = GetComponent<PlayerControl>();

        projectileCooldown = 1f;
        leftProjectileCD = 0f;
    }
    void Update()
    {
        active();
    }

        Inventory invenSlot;
        int itemCode;

        GameObject itemObj;
    void active(){
        if(!Input.GetKeyDown(KeyCode.X)) return;

        invenSlot = invenManager.quickslot[pCtrl.slotSelectNumber-1];
        if(invenSlot.count == 0) return;
        itemCode = invenSlot.itemCode;

        if(itemCode == 0 && state.actionable() && invenSlot.cooldown <= 0){
            invenManager.deleteItem(pCtrl.slotSelectNumber-1, 1);
            StartCoroutine(invenManager.cooldown("projectile", 1f));

            itemObj = Instantiate(itemObjList[itemCode], transform.position, Quaternion.identity, effectSystem.transform);

            itemObj.transform.Find("light2D").GetComponent<Light2D>().intensity = 1 - globalLight.GetComponent<Light2D>().intensity;




            int dir;
            if(transform.GetComponent<SpriteRenderer>().flipX) dir = -1;
            else dir = 1;

            itemObj.transform.position += new Vector3(dir*0.5f, 0.5f, 0);
            if(dir == -1) itemObj.GetComponent<SpriteRenderer>().flipX = true;

            itemObj.GetComponent<Rigidbody2D>().velocity = new Vector2(dir*15f, 0);
        }
    }
}
