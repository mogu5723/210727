using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterState : MonoBehaviour
{
    GameObject WSCanvas, textObj;

    SpriteRenderer rend;

    Image hpBar; public int hp, maxHp; 
    public bool deadState;



    private void Awake() {
        WSCanvas = transform.parent.parent.GetComponent<DataSetting>().WSCanvas;
        textObj = transform.parent.parent.GetComponent<DataSetting>().textObj;
        
        rend = GetComponent<SpriteRenderer>();

        hpBar = transform.Find("Canvas").Find("hpbar").Find("bghp").Find("hp").GetComponent<Image>();
    }

    
    void Update()
    {
        
    }

    public void damaged(float damage){
        if(deadState) return;

        hp -= (int)damage;
        
        if(hp <= 0) {
            damage += hp;
            hp = 0;
            StartCoroutine(dead());
        }
        hpBar.fillAmount = hp/(float)maxHp;

    
        if(damage > 0) StartCoroutine(damagedText((int)damage));   
    }

    IEnumerator dead(){
        deadState = true;

        transform.Find("Canvas").Find("hpbar").gameObject.SetActive(false);

        Color temp = new Color(1f, 1f, 1f, 1f);
        while(temp.a > 0){
            rend.color = temp;
            temp.a -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }


    IEnumerator damagedText(int damage){
        GameObject text = Instantiate(textObj, transform.position, Quaternion.identity);
        text.transform.SetParent(WSCanvas.transform);

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
}
