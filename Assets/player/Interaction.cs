using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interaction : MonoBehaviour
{
    public InventoryManager invenManager;
    public GameObject WSCanvas; GameObject textObj; List<Coroutine> deadStopCoroutines;

    Animator anim;
    PlayerState state;
    
    private void Awake() {
        anim = GetComponent<Animator>();
        state = GetComponent<PlayerState>();
        textObj = WSCanvas.transform.Find("Text").gameObject;
        deadStopCoroutines = new List<Coroutine>();
    }
    
    void Update()
    {
        
    }

    public List<GameObject> interactObj;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "mineral"){
            interactObj.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "mineral"){
            interactObj.Remove(other.gameObject);
        }
    }

    GameObject GetCloseObject(){
        GameObject temp;
        float minDistance, distance;
        float posX;

        posX = interactObj[0].transform.position.x + interactObj[0].GetComponent<BoxCollider2D>().size.x/2f-0.5f;
        distance = minDistance = Mathf.Abs(transform.position.x - posX);
        temp = interactObj[0];
        foreach(GameObject obj in interactObj){
            posX = obj.transform.position.x + obj.GetComponent<BoxCollider2D>().size.x/2f-0.5f;
            distance = Mathf.Abs(transform.position.x - posX);
            if(minDistance > distance) minDistance = distance;
            temp = obj;
        }

        return temp;
    }

    public void Interact(){
        if(Input.GetKeyDown(KeyCode.Space) && state.stand && state.actionable() && interactObj.Count > 0){
            GameObject obj = GetCloseObject();

            if(obj.name == "rock") StartCoroutine(mining(obj));
        }
    }

    public IEnumerator mining(GameObject obj){
        state.isInteractive = true;
        anim.SetInteger("animNumber", 4);
        obj.transform.Find("Canvas").Find("hpbar").gameObject.SetActive(true);

        Image hpbar = obj.transform.Find("Canvas").Find("hpbar").Find("bghp").Find("hp").GetComponent<Image>();
        float maxHP = 10f; float hp = maxHP, playerHP = state.hp;

        while(Input.GetKey(KeyCode.Space)){
            hp -= state.miningPower * Time.deltaTime;
            if(hp <= 0){
                hp = maxHP;
                deadStopCoroutines.Add(StartCoroutine(textup("+1", Color.white, obj, 5)));
                invenManager.addItem(0, "projectile", 1, 99);
            }
            hpbar.fillAmount = hp/maxHP;
            yield return null;

            if(state.stunState) break;
            if(state.knockbackState) break;
        }

        obj.transform.Find("Canvas").Find("hpbar").gameObject.SetActive(false);
        state.isInteractive = false;
    }
    IEnumerator textup(string str, Color color, GameObject obj, int size){
        Vector2 upPos;
        if(obj.GetComponent<BoxCollider2D>() != null){
            BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
            upPos = new Vector2(obj.transform.position.x + col.offset.x, obj.transform.position.y + col.offset.y + col.size.y/2f);
        }else{
            upPos = obj.transform.position;
        }
        
        GameObject text = Instantiate(textObj, upPos, Quaternion.identity);
        state.textManager.textList.Add(text);

        Text textGUI = text.GetComponent<Text>();
        Rigidbody2D TRigid = text.GetComponent<Rigidbody2D>();
        text.transform.SetParent(WSCanvas.transform);

        text.transform.localScale = new Vector3(1f,1f,1f);
        textGUI.text = str; textGUI.color = color; textGUI.fontSize = size;

        text.SetActive(true);

        TRigid.gravityScale = 0; TRigid.velocity = new Vector2(0, 1f);

        Color temp = color;
        while(temp.a > 0){
            yield return null;
            temp.a -= Time.deltaTime;
            textGUI.color = temp;
        }

        state.textManager.textList.Remove(text);
        Destroy(text);
    }

    public void deadStopCoroutine(){
        int count = deadStopCoroutines.Count;
        for(int i = 0; i < count; i++){
            StopCoroutine(deadStopCoroutines[0]);
            deadStopCoroutines.RemoveAt(0);
        }
    }
}
