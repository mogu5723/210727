using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ItemControl : MonoBehaviour
{
    PlayerState state;
    PlayerControl pCtrl;
    SpriteRenderer rend;

    public InventoryManager invenManager;

    public GameObject[] itemObjList;
    public GameObject globalLight; public GameObject effectSystemObj; EffectSystem effectSystem;

    public float projectileCooldown, leftProjectileCD;

    private void Awake() {
        state = GetComponent<PlayerState>();
        pCtrl = GetComponent<PlayerControl>();
        rend = GetComponent<SpriteRenderer>();
        effectSystem = effectSystemObj.GetComponent<EffectSystem>();

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
        if(!Input.GetKeyDown(KeyCode.X) || state.isAttacking) return;

        invenSlot = invenManager.inven[pCtrl.slotSelectNumber-1];
        if(invenSlot.count == 0) return;
        itemCode = invenSlot.itemCode;

        if(itemCode == 0 && state.actionable() && invenSlot.cooldown <= 0 && state.stand){ //돌 던지기
            invenManager.deleteItem(pCtrl.slotSelectNumber-1, 1);
            invenManager.StartCoroutine(invenManager.cooldown("projectile", 2f));
            state.isInteractive = true;
            pCtrl.anim.SetInteger("animNumber", 0);
            Invoke("offInteraction", 0.1f);

            effectSystem.tempObjList.Add(itemObj = Instantiate(itemObjList[itemCode], transform.position, Quaternion.identity, effectSystemObj.transform));

            int dir;
            if(transform.GetComponent<SpriteRenderer>().flipX) dir = -1;
            else dir = 1;
            itemObj.transform.position += new Vector3(0, 0.5f, 0);
            if(dir == -1) itemObj.GetComponent<SpriteRenderer>().flipX = true;
            itemObj.GetComponent<Rigidbody2D>().velocity = new Vector2(dir*12f, 0);
        }
        else if(itemCode == 1 && state.actionable()){ //움직일 수 있는 바위 놓기
            RaycastHit2D hit;
            int raymask = 1 + (1<<7) + (1<<9);
            hit = Physics2D.Raycast(transform.position+new Vector3(0, 0.5f, 0), new Vector2(rend.flipX ? -1 : 1, 0), 1f, raymask);
            if(hit.collider != null) return;

            invenManager.deleteItem(pCtrl.slotSelectNumber-1, 1);

            GameObject mapobj = state.CCtrl.chapter[state.CCtrl.mapCode[0]].Map[state.CCtrl.mapCode[1]];
            GameObject clone;
            clone = Instantiate(itemObjList[itemCode], transform.position+new Vector3(rend.flipX ? -1f : 1f , 0.5f), Quaternion.identity, mapobj.transform);
            clone.name = "movableStone";
        }
    }

    public void offInteraction(){
        state.isInteractive = false;
    }
}
