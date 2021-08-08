using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1_1act : MonoBehaviour
{
    public MonsterState state;
    Rigidbody2D rigid; BoxCollider2D col;
    SpriteRenderer rend;
    Animator anim;

    PlayerState PState;

    Coroutine moveCoroutine;

    private void Awake() {
        state = GetComponent<MonsterState>();
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    private void OnEnable() {
        stateSetting();
        moveCoroutine = StartCoroutine(move0());
    }

    void stateSetting(){    //몬스터 기본 셋팅
        col.enabled = true;
        rigid.gravityScale = 8;

        state.hp = state.maxHp = 30;
        state.stunTime = 0;
        state.deadState = false;
        state.moveSpeed = 2;
        rend.enabled = true;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(state.stunTime <= 0 && moveCoroutine == null && !state.deadState){
            moveCoroutine = StartCoroutine(move0());
        }
    }

    RaycastHit2D hit; int raymask;
    float movingTargetX, movingDistanceX;
    int dir;
    IEnumerator move0(){
        if(Random.Range(0f, 1f) > 0.5) dir = 1;
        else dir = -1;
        raymask = 1 << 7;

        while(true){
            yield return new WaitForFixedUpdate();
            for (int i = 0; i < 2; i++){    //오른쪽, 왼쪽 이동공간 탐색
                movingTargetX = transform.position.x + Random.Range(0.5f, 1.5f) * dir;
                movingDistanceX = Mathf.Abs(movingTargetX - transform.position.x);

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
                if(state.stunTime <= 0 && !state.deadState)
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                else break;

                yield return new WaitForSeconds(1f);
                if(Random.Range(0f, 1f) > 0.5) dir = 1;
                else dir = -1;
                continue;
            }

            while (transform.position.x * dir < movingTargetX * dir){  //지정좌표까지 이동 무언가 막혔을 경우 방향 전환 후 다시 탐색
                if(state.stunTime <= 0 && !state.deadState)
                    rigid.velocity = new Vector2(dir * state.moveSpeed, rigid.velocity.y);
                else break;

                yield return new WaitForFixedUpdate();
                if(Mathf.Abs(rigid.velocity.x) < 0.05f) {
                    dir *= -1;
                    break;
                }
            }
            if(state.stunTime > 0) break;

            if(Random.Range(0, 1f) < 0.1) dir *= -1;
        }

        moveCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !state.deadState){
            PState = other.gameObject.GetComponent<PlayerState>();
            PState.damaged(5f);
            PState.stun(0.3f);
            PState.knockback(transform.position, 10f, 3);
        }
    }
}
