using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Mob1_4AI : MonoBehaviour
{
    public MonsterState state;
    BoxCollider2D col;
    Rigidbody2D rigid;
    SpriteRenderer rend;
    Animator anim;
    GameObject effectSystemObj; EffectSystem effectSystem;
    public PlayerState PState;

    //몬스터 개인 변수
    Coroutine actCoroutine;
    GameObject lightObj, lightObj1; public Light2D light0, light1; PolygonCollider2D laserCol0;
    bool isPlayerDetected;
    LineRenderer lineRenderer1, lineRenderer2;
    public GameObject[] expression;
    BoxCollider2D col1;
    

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
        lightObj = transform.Find("Light").gameObject; lightObj1 = transform.Find("Light1").gameObject;
        light0 = lightObj.GetComponent<Light2D>(); light1 = lightObj1.GetComponent<Light2D>();
        lineRenderer1 = transform.Find("lineRend1").GetComponent<LineRenderer>();
        lineRenderer2 = transform.Find("lineRend2").GetComponent<LineRenderer>();
        laserCol0 = transform.Find("lineRend2").GetComponent<PolygonCollider2D>();
        col1 = transform.Find("collider2").GetComponent<BoxCollider2D>();
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
        lightObj.SetActive(false); lightObj1.SetActive(false);
        isPlayerDetected = false;
        rend.flipX = false;
        lineRenderer1.enabled = false; lineRenderer2.enabled = false;
        lightObj.SetActive(false); lightObj1.SetActive(false);
        laserCol0.enabled = false;
        col.isTrigger = false;
        rigid.gravityScale = 1;
        anim.SetInteger("animNumber", 0);
        col1.offset = new Vector2(0.0399f, 1.4288f);
        col1.size = new Vector2(1.6452f, 2.8576f);
    }

    void Update(){
        if(state.isDead){
            if(actCoroutine != null) StopCoroutine(actCoroutine);
            lightObj.SetActive(false); lightObj1.SetActive(false);
            rigid.velocity = Vector2.zero;
        }    

        if(!isPlayerDetected && PState.transform.position.x < -41){
            isPlayerDetected = true;
            actCoroutine = StartCoroutine(attack0());
        }
    }

    private void FixedUpdate() {
        
    }

    IEnumerator attack0(){
        int dir = (rend.flipX ? -1 : 1);
        List<Vector2> laserPoints = new List<Vector2>();
        yield return new WaitForSeconds(1f);

        while(true){
            int rand = Random.Range(0, 3);
            if(rand == 0){
                state.ignoreKnockback = true;
                rend.flipX = PState.transform.position.x - transform.position.x > 0 ? false : true;
                dir = (rend.flipX ? -1 : 1);
                lightObj1.SetActive(true); lightObj1.transform.position = new Vector3(transform.position.x + dir * 0.53125f, transform.position.y + 2.53125f, 0); light1.intensity = 0;

                for (int i = 0; i < 50; i++){
                    if(i < 26)
                        effectSystem.collectingEffect(lightObj1.transform.position, new Color(1, 1, 1, 0), 2f, 0.5f, 1f);
                    light1.intensity += 0.1f;
                    yield return new WaitForFixedUpdate();
                }

                lineRenderer1.enabled = true; lineRenderer2.enabled = true;
                lineRenderer1.SetPosition(0, lightObj1.transform.position);
                lineRenderer1.SetPosition(1, lineRenderer1.GetPosition(0) + new Vector3(0, -10, 0));
                lineRenderer2.SetPosition(0, lightObj1.transform.position);
                lineRenderer2.SetPosition(1, lineRenderer1.GetPosition(0) + new Vector3(0, -10, 0));
                lightObj.SetActive(true);
                lightObj.transform.position = lineRenderer1.GetPosition(1);


                float posX = lineRenderer1.GetPosition(0).x, posY = -6;
                float width0 = 0.5f;

                laserPoints.Clear();
                laserPoints.Add(lineRenderer1.GetPosition(0) - transform.position + new Vector3(-width0, 0));
                laserPoints.Add(lineRenderer1.GetPosition(0) - transform.position + new Vector3(width0, 0));
                laserPoints.Add(lineRenderer1.GetPosition(1) - transform.position + new Vector3(width0, 0));
                laserPoints.Add(lineRenderer1.GetPosition(1) - transform.position + new Vector3(-width0, 0));
                laserCol0.SetPath(0, laserPoints);
                laserCol0.enabled = true;

                while (true){
                    yield return new WaitForFixedUpdate();
                    posX += dir * Time.deltaTime * 10f;

                    if (posX < -63f || -42f < posX) break;

                    lineRenderer1.SetPosition(1, new Vector3(posX, -6, 0));
                    lineRenderer2.SetPosition(1, new Vector3(posX, -6, 0));
                    lightObj.transform.position = new Vector3(posX, -6, 0);
                    laserPoints[2] = lineRenderer1.GetPosition(1) - transform.position + new Vector3(width0, 0);
                    laserPoints[3] = lineRenderer1.GetPosition(1) - transform.position + new Vector3(-width0, 0);
                    laserCol0.SetPath(0, laserPoints);

                    if (Random.Range(0, 1) < 0.1f)
                        effectSystem.squareFragmentEffect(1, new Color(38 / 256f, 78 / 256f, 133 / 256f, 1), 10f, lightObj.transform.position + new Vector3(0, 0.1f, 0));
                }
                while (posY < -3){
                    yield return new WaitForFixedUpdate();
                    posY += Time.deltaTime * 10f;

                    lineRenderer1.SetPosition(1, new Vector3(posX, posY, 0));
                    lineRenderer2.SetPosition(1, new Vector3(posX, posY, 0));
                    lightObj.transform.position = new Vector3(posX, posY, 0);
                    laserPoints[2] = lineRenderer1.GetPosition(1) - transform.position + new Vector3(0, width0);
                    laserPoints[3] = lineRenderer1.GetPosition(1) - transform.position + new Vector3(0, -width0);
                    laserCol0.SetPath(0, laserPoints);

                    if (Random.Range(0, 1) < 0.1f)
                        effectSystem.squareFragmentEffect(1, new Color(38 / 256f, 78 / 256f, 133 / 256f, 1), 10f, lightObj.transform.position + new Vector3(-0.1f*dir, 0, 0));
                }
                lineRenderer1.enabled = false; lineRenderer2.enabled = false;
                lightObj.SetActive(false); lightObj1.SetActive(false);
                laserCol0.enabled = false;

                yield return new WaitForSeconds(0.5f);

                state.ignoreKnockback = false;
            }
            else if(rand == 1){
                state.ignoreKnockback = true;
                rend.flipX = PState.transform.position.x - transform.position.x > 0 ? false : true;
                dir = (rend.flipX ? -1 : 1);
                yield return new WaitForFixedUpdate();
                rigid.velocity = new Vector2(0, 7f);


                while(transform.position.y > -5.9f || rigid.velocity.y > 0){
                    yield return new WaitForFixedUpdate();
                }

                effectSystem.squareFragmentEffect(20, new Color(38 / 256f, 78 / 256f, 133 / 256f, 1), 30, transform.position + new Vector3(0, 0.1f, 0));
                rigid.velocity = new Vector2(10*dir, 0);
                anim.SetInteger("animNumber", 1);
                col1.size = col.size;
                col1.offset = col.offset;

                for(int i = 0; i < 3; i++){
                    while(Mathf.Abs(rigid.velocity.x) > 1){
                        rigid.velocity = new Vector2(10*dir, 0);
                        yield return new WaitForFixedUpdate();
                    }
                    effectSystem.squareFragmentEffect(20, new Color(38 / 256f, 78 / 256f, 133 / 256f, 1), 20, transform.position + new Vector3(dir, 1f, 0));
                    dir *= -1;
                    rend.flipX = !rend.flipX; 
                    rigid.velocity = new Vector2(10*dir, 0);
                    yield return new WaitForFixedUpdate();
                }

                for(int i = 0; i < 10; i++){
                    rigid.velocity = new Vector2(10*dir, 0);
                    yield return new WaitForFixedUpdate();
                }

                yield return new WaitForSeconds(1f);
                rigid.velocity = Vector2.zero;
                anim.SetInteger("animNumber", 0);
                col1.offset = new Vector2(0.0399f, 1.4288f);
                col1.size = new Vector2(1.6452f, 2.8576f);

                state.ignoreKnockback = false;
            }
            else if(rand == 2){
                col.isTrigger = true;
                rigid.gravityScale = 0;
                
                anim.SetInteger("animNumber", 2);
                while(transform.position.y < 5){
                    yield return new WaitForFixedUpdate();
                    rigid.velocity = new Vector2(0, 10);
                }
                rigid.velocity = Vector2.zero;

                float time = 0;
                Vector2 spawnPoint = new Vector2(0, 14);
                int randValue;
                while(time < 10){
                    yield return new WaitForFixedUpdate();
                    time += 0.02f;
                    rigid.velocity = new Vector2(0, Mathf.Sin(time*2));

                    spawnPoint.x = Random.Range(-64.5f, -34.5f);
                    randValue = Random.Range(0, 10);
                    if(randValue < 6) randValue = 0;
                    else if(randValue < 9) randValue = 1;
                    else randValue = 2;
                    
                    if((int)(time*100) % 10 == 1)
                        effectSystem.tempObjList.Add(Instantiate(expression[randValue], spawnPoint, Quaternion.identity, effectSystem.transform));
                }
                
                rigid.gravityScale = 1;

                while(transform.position.y > -5){
                    yield return new WaitForFixedUpdate();
                }

                col.isTrigger = false;
                anim.SetInteger("animNumber", 0);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
