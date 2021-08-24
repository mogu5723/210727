using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Mob1_3AI : MonoBehaviour
{
    MonsterState state;
    BoxCollider2D col;
    SpriteRenderer rend;
    Animator anim;
    GameObject effectSystemObj; EffectSystem effectSystem;
    PlayerState PState;

    //몬스터 개인 변수
    Coroutine actCoroutine;
    GameObject lightObj;
    

    private void Awake() {
        state = GetComponent<MonsterState>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

        effectSystemObj = transform.parent.parent.GetComponent<DataSetting>().EffectSystem;
        effectSystem = effectSystemObj.GetComponent<EffectSystem>();

        state.spawnX = transform.position.x; state.spawnY = transform.position.y;

        //개인 변수
        lightObj = transform.Find("Light").gameObject;
    }

    private void OnEnable() {
        stateSetting();
    }

    void stateSetting(){    //몬스터 기본 세팅
        col.enabled = true;

        transform.position = new Vector3(state.spawnX, state.spawnY, 0);

        state.hp = state.maxHp = 50;
        state.stunTime = 0; state.ignoreKnockback = true;
        state.isDead = false;
        state.moveSpeed = 3;
        rend.enabled = true;

        //개인 세팅
        actCoroutine = StartCoroutine(act0());
        lightObj.SetActive(false);
    }

    void Update(){
        if(state.isDead){
            if(actCoroutine != null) StopCoroutine(actCoroutine);
            lightObj.SetActive(false);
        }
    }

    IEnumerator act0(){
        rend.flipX = true;
        lightObj.transform.localScale = new Vector3(-1, 1, 1);
        while(true){
            rend.flipX = !rend.flipX;
            lightObj.transform.localScale = new Vector3(-lightObj.transform.localScale.x, 1, 1);
            lightObj.SetActive(false);
            anim.SetTrigger("animStart");
            

            yield return new WaitForSeconds(3f);

            lightObj.SetActive(true);

            yield return new WaitForSeconds(1f);
        }
    }
}
