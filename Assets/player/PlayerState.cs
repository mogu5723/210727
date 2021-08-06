using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour
{   
    public CamearControl CCtrl;
    Rigidbody2D rigid;
    public Image hpBar; public Text hpText;
    public GameObject WSCanvas; GameObject textObj;

    public int maxHp, hp;

    public float respawnX, respawnY; public int mapCode0, mapCode1;

    public float speed;

    public bool stand;
    public bool stunState; float stunTime;
    public bool knockbackState;
    public bool mining; public float miningPower;
    
    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        textObj = WSCanvas.transform.Find("Text").gameObject;

        respawn();
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
    }

    public void damaged(float damage){
        hp -= (int)damage;
        
        if(hp < 0) {
            damage += hp;
            hp = 0;
        }
        hpBar.fillAmount = hp/(float)maxHp;
        hpText.text = "HP "+hp+"/"+maxHp;

        if(hp <= 0 ) respawn();
        if(damage > 0) StartCoroutine(damagedText((int)damage));   
    }
    IEnumerator damagedText(int damage){
        GameObject text = Instantiate(textObj, transform.position, Quaternion.identity);
        text.transform.SetParent(WSCanvas.transform);

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
        Destroy(text);
    }

    void respawn(){
        hp = maxHp = 50;
        speed = 5.5f;
        damaged(0);
        stunState = false; stunTime = 0;
        knockbackState = false;
        mining = false; miningPower = 10f;
        CCtrl.PlayerSpawn(mapCode0, mapCode1, respawnX, respawnY);
    }

    public void stun(float t){
        stunTime += t;
        stunState = true;
    }
    public IEnumerator knockback(Vector3 v, float power, int mode){
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
        if(mining) return false;
        return true;
    }
}
