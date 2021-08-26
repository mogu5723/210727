using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Mob1_4AI : MonoBehaviour
{
    MonsterState state;
    BoxCollider2D col;
    Rigidbody2D rigid;
    SpriteRenderer rend;
    Animator anim;
    GameObject effectSystemObj; EffectSystem effectSystem;
    PlayerState PState;

    //몬스터 개인 변수
    Coroutine actCoroutine;
    GameObject lightObj; public Light2D light0;
    bool isMoving; bool isPlayerDetected;
    LineRenderer lineRenderer1, lineRenderer2;
    

    private void Awake() {
        state = GetComponent<MonsterState>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();

        effectSystemObj = transform.parent.parent.GetComponent<DataSetting>().EffectSystem;
        effectSystem = effectSystemObj.GetComponent<EffectSystem>();

        state.spawnX = transform.position.x; state.spawnY = transform.position.y;

        //개인 변수
        lightObj = transform.Find("Light").gameObject;
        light0 = lightObj.GetComponent<Light2D>();
        lineRenderer1 = transform.Find("lineRend1").GetComponent<LineRenderer>();
        lineRenderer2 = transform.Find("lineRend2").GetComponent<LineRenderer>();
    }

    private void Start() {
        PState = state.dataSetting.playerState;
    }

    private void OnEnable() {
        stateSetting();
    }

    void stateSetting(){    //몬스터 기본 세팅
        col.enabled = true;

        transform.position = new Vector3(state.spawnX, state.spawnY, 0);

        state.hp = state.maxHp = 500;
        state.stunTime = 0; state.ignoreKnockback = false;
        state.isDead = false;
        state.moveSpeed = 3;
        rend.enabled = true;
        state.minDropCoin = 3; state.maxDropCoin = 5;
        state.collisionDamage = 10;

        //개인 세팅
        //actCoroutine = StartCoroutine(act0());
        lightObj.SetActive(false);
        isMoving = false; isPlayerDetected = false;
        rend.flipX = false;
        lineRenderer1.enabled = false; lineRenderer2.enabled = false;
        lightObj.SetActive(false);
    }

    void Update(){
        if(state.isDead){
            if(actCoroutine != null) StopCoroutine(actCoroutine);
            lightObj.SetActive(false);
        }    

        if(!isPlayerDetected && PState.transform.position.x < -41){
            isPlayerDetected = true;
            isMoving = true;
            actCoroutine =  StartCoroutine(attack0());
        }
    }

    private void FixedUpdate() {
        if(isPlayerDetected && isMoving && Mathf.Abs(PState.transform.position.x - transform.position.x) > 1 && state.stunTime <= 0){
            rend.flipX = PState.transform.position.x - transform.position.x > 0 ? false : true;
            rigid.velocity = new Vector2(rend.flipX ? -state.moveSpeed : state.moveSpeed, rigid.velocity.y);
        }
    }

    IEnumerator attack0(){
        int dir = (rend.flipX ? -1 : 1);

        while(true){
            yield return new WaitForSeconds(3f);

            isMoving = false;
            state.ignoreKnockback = true;

            yield return new WaitForSeconds(1f);

            dir = (rend.flipX ? -1 : 1);
            lineRenderer1.enabled = true; lineRenderer2.enabled = true;
            lineRenderer1.SetPosition(0, new Vector3(transform.position.x + dir*0.7f, transform.position.y + 1.9f, 0));
            lineRenderer1.SetPosition(1, lineRenderer1.GetPosition(0) + new Vector3(0, -10, 0));
            lineRenderer2.SetPosition(0, new Vector3(transform.position.x + dir*0.7f, transform.position.y + 1.9f, 0));
            lineRenderer2.SetPosition(1, lineRenderer1.GetPosition(0) + new Vector3(0, -10, 0));
            lightObj.SetActive(true);
            lightObj.transform.position = lineRenderer1.GetPosition(1);

            //float angle = 0; 
            float posX = lineRenderer1.GetPosition(0).x;
            while (true){
                yield return new WaitForFixedUpdate();
                //angle += Time.deltaTime * Mathf.PI * 0.5f * dir;

                //posX = lineRenderer1.GetPosition(0).x + Mathf.Tan(angle)*2;
                posX += dir * Time.deltaTime*10f;

                if(posX < -63f || -42f < posX) break;
    
                lineRenderer1.SetPosition(1, new Vector3(posX, -6, 0));
                lineRenderer2.SetPosition(1, new Vector3(posX, -6, 0));
                lightObj.transform.position = new Vector3(posX, -6, 0);
                if(Random.Range(0, 1) < 0.1f)
                    effectSystem.squareFragmentEffect(1, new Color(38/256f, 78/256f, 133/256f, 1), 10f, lightObj.transform.position + new Vector3(0, 0.1f, 0));
            }
            lineRenderer1.enabled = false; lineRenderer2.enabled = false;
            lightObj.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            isMoving = true;
            state.ignoreKnockback = false;
        }
    }
}
