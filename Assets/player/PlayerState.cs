using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    Rigidbody2D rigid;
    public Image hpBar;

    int maxHp, hp;

    public bool stunState; float stunTime;
    
    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        hp = maxHp = 50;
        stunState = false;
        stunTime = 0;
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
        hpBar.fillAmount = hp/(float)maxHp;
        if(hp <= 0) {
            hp = 0;
            gameObject.SetActive(false);
        }
    }

    public void stun(float t){
        stunTime += t;
        stunState = true;
    }

    public IEnumerator knockback(Vector3 v, float power, int mode){
        if(mode == 1){
            v = transform.position - v;
        }else if(mode == 2){
            if(v.x > 0) v = new Vector3(1f, 0, 0);
            else v = new Vector3(-1f, 0, 0);
        }else if(mode == 3){
            if(transform.position.x - v.x > 0) v = new Vector3(1f, 0, 0);
            else v = new Vector3(-1f, 0, 0);
        }

        while (power > 0){
            transform.Translate(v.normalized * power * Time.deltaTime);
            power -= Time.deltaTime * 10;
            yield return new WaitForFixedUpdate();
        }
    }
}
