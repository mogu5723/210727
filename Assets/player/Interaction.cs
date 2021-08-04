using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interaction : MonoBehaviour
{
    public InventoryManager invenManager;
    public GameObject WSCanvas; GameObject textObj;

    Animator anim;
    PlayerState state;
    
    private void Awake() {
        anim = GetComponent<Animator>();
        state = GetComponent<PlayerState>();
        textObj = WSCanvas.transform.Find("Text (TMP)").gameObject;
    }
    
    void Update()
    {
        
    }

    public IEnumerator mining(GameObject obj){
        state.mining = true;
        anim.SetInteger("animNumber", 4);
        obj.transform.Find("Canvas").Find("hpbar").gameObject.SetActive(true);

        Image hpbar = obj.transform.Find("Canvas").Find("hpbar").Find("bghp").Find("hp").GetComponent<Image>();
        float maxHP = 10f; float hp = maxHP, playerHP = state.hp;

        while(Input.GetKey(KeyCode.Space)){
            hp -= state.miningPower * Time.deltaTime;
            if(hp <= 0){
                hp = maxHP;
                StartCoroutine(textup("+1", Color.white, obj, 12));
                invenManager.addItem(0, 1, 99);
            }
            hpbar.fillAmount = hp/maxHP;
            yield return null;

            if(state.stunState) break;
            if(state.knockbackState) break;
        }

        obj.transform.Find("Canvas").Find("hpbar").gameObject.SetActive(false);
        state.mining = false;
    }
    IEnumerator textup(string str, Color color, GameObject obj, float size){
        Vector2 upPos;
        if(obj.GetComponent<BoxCollider2D>() != null){
            BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
            upPos = new Vector2(obj.transform.position.x + col.offset.x, obj.transform.position.y + col.offset.y + col.size.y/2f);
        }else{
            upPos = obj.transform.position;
        }
        
        GameObject text = Instantiate(textObj, upPos, Quaternion.identity);

        TextMeshProUGUI textGUI = text.GetComponent<TextMeshProUGUI>();
        Rigidbody2D TRigid = text.GetComponent<Rigidbody2D>();
        text.transform.SetParent(WSCanvas.transform);

        text.transform.localScale = new Vector3(1f,1f,1f);
        textGUI.text = str; textGUI.color = color; textGUI.fontSize = size;

        text.SetActive(true);

        TRigid.gravityScale = 0; TRigid.velocity = new Vector2(0, 1f);

        Color temp = color;
        while(temp.a > 0){
            yield return null;
            temp.a -= Time.deltaTime*2;
            textGUI.color = temp;
        }

        Destroy(text);
    }
}
