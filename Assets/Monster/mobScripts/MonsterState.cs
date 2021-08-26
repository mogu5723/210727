using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MonsterState : MonoBehaviour
{
    //관련 오브젝트 or 컴포넌트
    public DataSetting dataSetting; TextManager textManager; GameObject WSCanvas, textObj;
    GameObject effectObj; EffectSystem effectSystem; DropSystem dropSystem;

    Rigidbody2D rigid; Collider2D col; 
    SpriteRenderer rend; Material matWhite, matDefault;

    //스텟
    public float spawnX, spawnY;
    Image hpBar; public int hp, maxHp; 
    public float moveSpeed;
    public float collisionDamage;
    //상태
    public float stunTime; public bool ignoreKnockback;
    public bool isDead;

    public int minDropCoin, maxDropCoin;


    private void Awake() {
        dataSetting = transform.parent.parent.GetComponent<DataSetting>();
        WSCanvas = dataSetting.WSCanvas;
        textObj = dataSetting.textObj;
        textManager = WSCanvas.GetComponent<TextManager>();
        effectObj = dataSetting.EffectSystem;
        effectSystem = effectObj.GetComponent<EffectSystem>();
        dropSystem = effectObj.GetComponent<DropSystem>();
        
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
        matDefault = rend.material;
        matWhite = dataSetting.matWhite;

        hpBar = transform.Find("Canvas").Find("hpbar").Find("bghp").Find("hp").GetComponent<Image>();
        minDropCoin = 3; maxDropCoin = 9;
    }

    private void OnEnable() {
        rend.material = matDefault;
        rend.color = new Color(1,1,1,1);
    }

    void Update()
    {
        if(stunTime > 0 && !ignoreKnockback){
            stunTime -= Time.deltaTime;
            if(stunTime <= 0) 
                stunTime = 0;
        }
    }

    public void damaged(float damage){
        if(isDead) return;

        hp -= (int)damage;
        rend.material = matWhite;
        Invoke("setMatDefault", 0.15f);
        
        if(hp <= 0) {
            damage += hp;
            hp = 0;

            StartCoroutine(dead());
        }
        hpBar.fillAmount = hp/(float)maxHp;

    
        if(damage > 0) StartCoroutine(damagedText((int)damage));   
    }

    void setMatDefault(){
        rend.material = matDefault;
    }

    
    IEnumerator dead(){
        isDead = true;

        transform.Find("Canvas").Find("hpbar").gameObject.SetActive(false);
        col.enabled = false;
        rigid.gravityScale = 0;
        rigid.velocity = new Vector2(0, 0);
        dropSystem.coinDrop(minDropCoin, maxDropCoin, transform);

        Color temp = new Color(1f, 1f, 1f, 1f);
        while(temp.a > 0){
            rend.color = temp;
            temp.a -= Time.deltaTime;
            yield return null;
        }

        rend.color = new Color(1f, 1f, 1f, 1f);
        rend.enabled = false;
    }

    IEnumerator damagedText(int damage){
        GameObject text = Instantiate(textObj, transform.position, Quaternion.identity);
        text.transform.SetParent(WSCanvas.transform);
        textManager.textList.Add(text);

        Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        text.transform.Translate(dir);
        text.transform.localScale = new Vector3(0.625f, 0.625f, 0.625f);
        text.GetComponent<Rigidbody2D>().gravityScale = 4;
        text.GetComponent<Text>().color = new Color(1f, 1f, 1f);
        text.GetComponent<Text>().text = "-"+damage;
        text.transform.localScale = new Vector3(1f,1f,1f);
        text.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        text.SetActive(true);
        text.GetComponent<Rigidbody2D>().AddForce(dir*10, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        Destroy(text);
    }

    public void stun(float t){
        if(stunTime < t)
            stunTime = t;
    }

    public void knockback(Vector3 v, float power, int mode){
        if(ignoreKnockback) return;
        StartCoroutine(knockback_(v, power, mode));
    }
    IEnumerator knockback_(Vector3 v, float power, int mode){
        if(mode == 1){
            v = transform.position - v;
        }else if(mode == 2){
            if(v.x > 0) v = new Vector3(1f, 0, 0);
            else v = new Vector3(-1f, 0, 0);
        }else if(mode == 3){
            if(transform.position.x - v.x > 0) v = new Vector3(1f, 0, 0);
            else v = new Vector3(-1f, 0, 0);
        }

        rigid.velocity = new Vector2(0, rigid.velocity.y);
        rigid.AddForce(v.normalized * power * 300f, ForceMode2D.Impulse);
        while(stunTime > 0){
            yield return new WaitForFixedUpdate();
        }
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }
}
