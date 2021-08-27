using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{   
    //관련 컴포넌트, 오브젝트 등
    public GameObject gameDataObj; public DataManagement dataManagement; GameData gameData;
    public CamearControl CCtrl; Interaction interaction;
    Rigidbody2D rigid; SpriteRenderer rend;
    public Image hpBar; public Text hpText;
    public GameObject WSCanvas; GameObject textObj; public TextManager textManager;
    public UnityEvent onPlayerDead; public List<Coroutine> deadStopCoroutines;

    //스탯
    public int maxHp, hp; 
    public float speed;
    //상태
    public bool stand;
    public bool stunState; float stunTime; public bool knockbackState; bool isRespawning;
    public bool isInteractive; public float miningPower;
    public bool isAttacking; public int attackDir;
    public float invincibleTime;
    //스폰지점
    public float respawnX, respawnY; public int mapCode0, mapCode1;
    
    
    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        interaction = GetComponent<Interaction>();
        textObj = WSCanvas.transform.Find("Text").gameObject;
        textManager = WSCanvas.GetComponent<TextManager>();
        deadStopCoroutines = new List<Coroutine>();
        dataManagement = gameDataObj.GetComponent<DataManagement>();
        gameData = dataManagement._gameData;
    }
    private void OnEnable() {
        isAttacking = false;
        invincibleTime = 0;
        rend.color = Color.white;
    }
    void Start()
    {
        //로드안한 게임 시작
        dataManagement.gameData.mapCode0 = 0; dataManagement.gameData.mapCode1 = 0;
        dataManagement.gameData.spawnX = -34.5f; dataManagement.gameData.spawnY = -1f;
        respawn();
    }

    Coroutine blinkCoroutine;

    void Update()
    {
        if(stunTime > 0){
            stunTime -= Time.deltaTime;
            if(stunTime <= 0) {
                stunTime = 0;
                stunState = false;
            }
        }
        if(invincibleTime > 0){
            invincibleTime -= Time.deltaTime;
            if(invincibleTime <= 0){
                invincibleTime = 0;
                StopCoroutine(blinkCoroutine);
                rend.color = new Color(1,1,1,1);
            }
        }

        if(Input.GetKeyDown(KeyCode.O)){
            dataManagement.SaveGameData();
        }else if(Input.GetKeyDown(KeyCode.P)){
            dataManagement.LoadGameData();
        }
    }

    public void damaged(float damage){
        if(invincibleTime > 0) return;

        hp -= (int)damage;
        invincibleTime = 1f;
        blinkCoroutine = StartCoroutine(blink0());
        
        
        if(hp < 0) {
            damage += hp;
            hp = 0;
        }
        hpBar.fillAmount = hp/(float)maxHp;
        hpText.text = hp+"/"+maxHp;

        if(hp <= 0 ) {
            invincibleTime = 0f;
            onPlayerDead.Invoke();
            dataManagement.LoadGameData();
        }
        else if(damage > 0) deadStopCoroutines.Add(StartCoroutine(damagedText((int)damage)));   
    }

    IEnumerator blink0(){
        while(invincibleTime > 0){
            rend.color = new Color(1,1,1,0.5f);
            yield return new WaitForSeconds(0.125f);
            rend.color = new Color(1,1,1,1f);
            yield return new WaitForSeconds(0.125f);
        }
    }

    public void respawn(){
        deadStopCoroutine();
        interaction.deadStopCoroutine();
        hp = maxHp = 50;
        speed = 5.5f;
        damaged(0);
        stunState = true; stunTime = 1f;
        knockbackState = false; isRespawning = true;
        isInteractive = false; miningPower = 10f;
        isAttacking = false;

        gameData = dataManagement.gameData;
        CCtrl.PlayerSpawn(gameData.mapCode0, gameData.mapCode1, gameData.spawnX, gameData.spawnY);
        deadStopCoroutines.Add(StartCoroutine(respawn_()));
    }

    IEnumerator respawn_(){
        yield return new WaitForSeconds(1f);
        isRespawning = false;
    }
    

    public void stun(float t){
        if(isRespawning) return;
        if(invincibleTime > 0) return;

        stunTime += t;
        stunState = true;
    }
    public void knockback(Vector3 v, float power, int mode){
        if(isRespawning) return;
        if(invincibleTime > 0) return;

        StartCoroutine(knockback_(v, power, mode));
    }
    IEnumerator knockback_(Vector3 v, float power, int mode){
        knockbackState = true;

        if(mode == 1){
            v = transform.position - v;
        }else if(mode == 2 || mode == 4){
            if(v.x > 0) v = new Vector3(1f, 0, 0);
            else v = new Vector3(-1f, 0, 0);
        }else if(mode == 3 || mode == 5){
            if(transform.position.x - v.x > 0) v = new Vector3(1f, 0, 0);
            else v = new Vector3(-1f, 0, 0);
        }

        yield return new WaitForFixedUpdate();
        rigid.velocity = v.normalized*power;
        if(mode < 4) rigid.velocity += new Vector2(0, 10f);
    
        while(stunState){
            yield return null;
        }

        knockbackState = false;
    }
    public void offInteraction(){
        isInteractive = false;
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

    IEnumerator damagedText(int damage){
        GameObject text = Instantiate(textObj, transform.position, Quaternion.identity);
        text.transform.SetParent(WSCanvas.transform);
        textManager.textList.Add(text);

        Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        text.transform.Translate(dir);
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
}
