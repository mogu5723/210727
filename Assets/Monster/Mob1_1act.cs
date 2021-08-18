using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1_1act : MonoBehaviour
{
    public MonsterState state;
    Rigidbody2D rigid; BoxCollider2D col;
    SpriteRenderer rend;
    Animator anim;
    GameObject effectSystemObj; GameObject attackObj; EffectSystem effectSystem;

    PlayerState PState;

    Coroutine moveCoroutine; Coroutine attackCoroutine;

    int stateNumber; // 1 = move, 2 = attack

    private void Awake() {
        state = GetComponent<MonsterState>();
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

        effectSystemObj = transform.parent.parent.GetComponent<DataSetting>().EffectSystem;
        effectSystem = effectSystemObj.GetComponent<EffectSystem>();
        attackObj = transform.Find("Mob1_1Attack0").gameObject;

        state.spawnX = transform.position.x; state.spawnY = transform.position.y;
    }

    private void OnEnable() {
        stateSetting();
    }

    void stateSetting(){    //몬스터 기본 셋팅
        col.enabled = true;
        rigid.gravityScale = 8;

        transform.position = new Vector3(state.spawnX, state.spawnY, 0);

        state.hp = state.maxHp = 30;
        state.stunTime = 0;
        state.isDead = false;
        state.moveSpeed = 2;
        rend.enabled = true;

        moveCoroutine = StartCoroutine(move0());
        attackCoroutine = StartCoroutine(attack0());
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    RaycastHit2D hit; int raymask;
    float movingTargetX, movingDistanceX;
    int dir;
    IEnumerator move0(){
        if(Random.Range(0f, 1f) > 0.5) dir = 1;
        else dir = -1;

        while(true){
            stateNumber = 1;
            for (int i = 0; i < 2; i++){    //오른쪽, 왼쪽 이동공간 탐색
                movingTargetX = transform.position.x + Random.Range(0.5f, 1.0f) * dir;
                movingDistanceX = Mathf.Abs(movingTargetX - transform.position.x);

                raymask = 1 << 7;
                hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), new Vector3(dir, 0, 0), movingDistanceX, raymask);
                if (hit.collider != null && hit.collider.CompareTag("block")){
                    if (i == 0) dir *= -1;
                    else dir = 0;
                    continue;
                }
                hit = Physics2D.Raycast(new Vector3(movingTargetX, transform.position.y + 0.5f, 0), new Vector3(0, -1, 0), 1, raymask);
                if (hit.collider == null || !hit.collider.CompareTag("block")){
                    if (i == 0) dir *= -1;
                    else dir = 0;
                    continue;
                }else break;
            }

            if (dir == 1) rend.flipX = false;
            else if (dir == -1) rend.flipX = true;
            else if(Random.Range(0f, 1f) < 0.3f) rend.flipX = !rend.flipX;
            
            if(dir != 0) anim.SetInteger("animNumber", 2);
            else {  //이동공간 못찾은경우 1초 정지 후 다시 탐색
                anim.SetInteger("animNumber", 1);
                rigid.velocity = new Vector2(0, rigid.velocity.y);

                yield return new WaitForSeconds(1f);
                if(state.isDead) break;
                while(state.stunTime > 0 || stateNumber == 2) yield return null;

                if(Random.Range(0f, 1f) > 0.5) dir = 1;
                else dir = -1;
                continue;
            }

            while (transform.position.x * dir < movingTargetX * dir){  //지정좌표까지 이동 무언가 막혔을 경우 방향 전환 후 다시 탐색
                rigid.velocity = new Vector2(dir * state.moveSpeed, rigid.velocity.y);

                yield return new WaitForFixedUpdate();
                if(state.isDead) break;
                if(state.stunTime > 0 || stateNumber == 2) break;

                if (Mathf.Abs(rigid.velocity.x) < 0.05f){
                    dir *= -1;
                    break;
                }
            }
            if(state.isDead) break;

            while (state.stunTime > 0 || stateNumber == 2) yield return null;

            if(Random.Range(0, 1f) < 0.05) dir *= -1;
        }
    }

    IEnumerator attack0(){
        while(true){
            while(true){
                raymask = (1 << 7) + (1 << 3);
                hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), new Vector3(dir, 0, 0), 10f, raymask);

                if(hit.collider != null && hit.collider.CompareTag("Player")) break;
                yield return null;
                while(state.stunTime > 0) yield return null;
            }

            stateNumber = 2;
            anim.SetInteger("animNumber", 3);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            
            yield return new WaitForSeconds(1f);
            if(state.isDead) break;
            GameObject attackObjClone = Instantiate(attackObj, transform.position, Quaternion.identity, effectSystemObj.transform);
            effectSystem.tempObjList.Add(attackObjClone);
            attackObjClone.SetActive(true);
            attackObjClone.transform.position += new Vector3(dir*0.5f, 0.5f, 0);
            if(dir == -1) attackObjClone.GetComponent<SpriteRenderer>().flipX = true;
            attackObjClone.GetComponent<Rigidbody2D>().velocity = new Vector2(dir*10f, 0);

            yield return new WaitForSeconds(0.291f);
            stateNumber = 1;

            yield return new WaitForSeconds(3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !state.isDead){
            PState = other.gameObject.GetComponent<PlayerState>();
            PState.damaged(5f);
            PState.stun(0.3f);
            PState.knockback(transform.position, 10f, 3);
        }
    }
}
