using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    Animator anim;
    PlayerState state;
    
    private void Awake() {
        anim = GetComponent<Animator>();
        state = GetComponent<PlayerState>();
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
            }
            hpbar.fillAmount = hp/maxHP;
            yield return null;

            if(state.stunState) break;
            if(state.knockbackState) break;
        }

        obj.transform.Find("Canvas").Find("hpbar").gameObject.SetActive(false);
        state.mining = false;
    }
}
