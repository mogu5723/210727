using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour
{   
    public CamearControl CCtrl;
    Rigidbody2D rigid;
    public Image hpBar; public TextMeshProUGUI hpText;

    public int maxHp, hp;

    public float respawnX, respawnY; public int mapCode0, mapCode1;

    public float speed;

    public bool stand;
    public bool stunState; float stunTime;
    public bool knockbackState;
    public bool mining; public float miningPower;
    
    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();

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

    public void damaged(float damege){
        hp -= (int)damege;
        
        if(hp < 0) hp = 0;
        hpBar.fillAmount = hp/(float)maxHp;
        hpText.text = "HP "+hp+"/"+maxHp;

        if(hp <= 0 ) respawn();   
    }

    void respawn(){
        hp = maxHp = 50;
        speed = 5.5f;
        damaged(0);
        stunState = false; stunTime = 0;
        knockbackState = false;
        mining = false; miningPower = 3f;
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
