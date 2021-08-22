using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{   
    public GameObject gameDataObj; DataManagement dataManagement; GameData gameData;
    public CamearControl CCtrl; Interaction interaction;
    Rigidbody2D rigid;
    public Image hpBar; public Text hpText;
    public GameObject WSCanvas; GameObject textObj; public TextManager textManager;
    public UnityEvent onPlayerDead; public List<Coroutine> deadStopCoroutines;

    //스탯
    public int maxHp, hp; 
    public float speed;
    //상태
    public bool stand;
    public bool stunState; float stunTime; public bool knockbackState;
    public bool isInteractive; public float miningPower;
    public bool isAttacking; public int attackDir;
    //스폰지점
    public float respawnX, respawnY; public int mapCode0, mapCode1;
    
    
    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        interaction = GetComponent<Interaction>();
        textObj = WSCanvas.transform.Find("Text").gameObject;
        textManager = WSCanvas.GetComponent<TextManager>();
        deadStopCoroutines = new List<Coroutine>();
        dataManagement = gameDataObj.GetComponent<DataManagement>();
        gameData = dataManagement._gameData;
    }
    private void OnEnable() {
        isAttacking = false;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(stunTime > 0){
            stunTime -= Time.deltaTime;
            if(stunTime <= 0) {
                stunTime = 0;
                stunState = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.O)){
            dataManagement.SaveGameData();
        }else if(Input.GetKeyDown(KeyCode.P)){
            dataManagement.LoadGameData();
        }
    }

    public void damaged(float damage){
        hp -= (int)damage;
        
        if(hp < 0) {
            damage += hp;
            hp = 0;
        }
        hpBar.fillAmount = hp/(float)maxHp;
        hpText.text = hp+"/"+maxHp;

        if(hp <= 0 ) {
            deadStopCoroutine();
            interaction.deadStopCoroutine();
            onPlayerDead.Invoke();
            respawn();
        }
        else if(damage > 0) deadStopCoroutines.Add(StartCoroutine(damagedText((int)damage)));   
    }
    IEnumerator damagedText(int damage){
        GameObject text = Instantiate(textObj, transform.position, Quaternion.identity);
        text.transform.SetParent(WSCanvas.transform);
        textManager.textList.Add(text);

        Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        text.transform.Translate(dir);
        text.transform.localScale = new Vector3(0.625f, 0.625f, 0.625f);
        text.GetComponent<Rigidbody2D>().gravityScale = 4;
        text.GetComponent<Text>().text = "-"+damage;
        text.transform.localScale = new Vector3(1f,1f,1f);
        text.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        text.SetActive(true);
        text.GetComponent<Rigidbody2D>().AddForce(dir*10, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        textManager.textList.Remove(text);
        Destroy(text);
    }

    public void respawn(){
        hp = maxHp = 50;
        speed = 5.5f;
        damaged(0);
        stunState = false; stunTime = 0;
        knockbackState = false;
        isInteractive = false; miningPower = 10f;
        isAttacking = false;

        gameData = dataManagement.gameData;
        CCtrl.PlayerSpawn(gameData.mapCode0, gameData.mapCode1, gameData.spawnX, gameData.spawnY);
    }

    public void stun(float t){
        stunTime += t;
        stunState = true;
    }
    public void knockback(Vector3 v, float power, int mode){
        StartCoroutine(knockback_(v, power, mode));
    }
    IEnumerator knockback_(Vector3 v, float power, int mode){
        knockbackState = true;

        if(mode == 1){
            v = transform.position - v;
        }else if(mode == 2){
            if(v.x > 0) v = new Vector3(1f, 0, 0);
            else v = new Vector3(-1f, 0, 0);
        }else if(mode == 3){
            if(transform.position.x - v.x > 0) v = new Vector3(1f, 0, 0);
            else v = new Vector3(-1f, 0, 0);
        }

        yield return null;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        rigid.AddForce(v.normalized * power, ForceMode2D.Impulse);
        while(stunState){
            yield return null;
        }

        knockbackState = false;
    }
    public bool actionable(){
        if(stunState) return false;
        if(isInteractive) return false;
        return true;
    }
    void deadStopCoroutine(){
        int count = deadStopCoroutines.Count;
        for(int i = 0; i < count; i++){
            StopCoroutine(deadStopCoroutines[0]);
            deadStopCoroutines.RemoveAt(0);
        }
    }
}
