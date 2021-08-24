using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1_2AI : MonoBehaviour
{
    MonsterState state;
    Rigidbody2D rigid; BoxCollider2D col;
    SpriteRenderer rend;
    Animator anim;
    GameObject effectSystemObj; EffectSystem effectSystem;
    PlayerState PState;

    //개인 변수
    Coroutine moveCoroutine, attackCoroutine;
    float power;

    private void Awake() {
        state = GetComponent<MonsterState>();
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

        effectSystemObj = transform.parent.parent.GetComponent<DataSetting>().EffectSystem;
        effectSystem = effectSystemObj.GetComponent<EffectSystem>();

        state.spawnX = transform.position.x; state.spawnY = transform.position.y;
    }

    private void OnEnable() {
        stateSetting();
    }

    void stateSetting(){    //몬스터 기본 세팅
        col.enabled = true;
        rigid.gravityScale = 8;

        transform.position = new Vector3(state.spawnX, state.spawnY, 0);

        state.hp = state.maxHp = 70;
        state.stunTime = 0; state.ignoreKnockback = false;
        state.isDead = false;
        state.moveSpeed = 3;
        rend.enabled = true;
        dir = Random.Range(0f, 1f) > 0.5 ? 1 : -1; 

        //개인 세팅
        moveCoroutine = StartCoroutine(move0());
        power = 10;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(state.isDead){
            if(moveCoroutine != null) StopCoroutine(moveCoroutine);
            if(attackCoroutine != null) StopCoroutine(attackCoroutine);
        }
    }

    RaycastHit2D hit; int raymask;
    float movingTargetX, movingDistanceX;
    int dir;
    IEnumerator move0(){
        if (Random.Range(0, 1f) < 0.05) dir *= -1;
        state.moveSpeed = 3;
        power = 10f;

        for (int i = 0; i < 2; i++){    //오른쪽, 왼쪽 이동공간 탐색
            if(IsMovable()) break;
            else dir =  i == 0 ? -dir : 0;
        }

        if (dir == 1) { rend.flipX = false; anim.SetInteger("animNumber", 1); }
        else if (dir == -1) { rend.flipX = true; anim.SetInteger("animNumber", 1); }
        else if (Random.Range(0f, 1f) < 0.3f) rend.flipX = !rend.flipX;

        if (dir == 0){  //이동공간 못찾은경우 1초 정지 후 다시 탐색
            dir = rend.flipX ? -1 : 1;
            anim.SetInteger("animNumber", 0);
            rigid.velocity = new Vector2(0, rigid.velocity.y);

            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 5; j++) yield return new WaitForFixedUpdate();
                if (PlayerCheck(5f)) {
                    attackCoroutine = StartCoroutine(attack0());
                    break;
                }
            }
        }
        else{   //지정좌표까지 이동 무언가 막혔을 경우 방향 전환 후 다시 탐색
            while (transform.position.x * dir < movingTargetX * dir){  
                rigid.velocity = new Vector2(dir * state.moveSpeed, rigid.velocity.y);

                yield return new WaitForFixedUpdate();
                if (state.stunTime > 0) break;

                if (Mathf.Abs(rigid.velocity.x) < 0.05f)
                {
                    dir *= -1; break;
                }
            }
        }
        while (state.stunTime > 0) yield return null;

        if (PlayerCheck(8f)) {
            attackCoroutine = StartCoroutine(attack0());
        }else moveCoroutine = StartCoroutine(move0());
    }

    IEnumerator attack0(){
        rend.flipX = (dir == 1 ? false : true);
        anim.SetInteger("animNumber", 0);
        yield return new WaitForSeconds(0.5f);

        anim.SetInteger("animNumber", 2);
        state.moveSpeed = 8;
        power = 15f;
        state.ignoreKnockback = true;

        while(IsMovable()){
            while (transform.position.x * dir < movingTargetX * dir){  
                rigid.velocity = new Vector2(dir * state.moveSpeed, rigid.velocity.y);

                yield return new WaitForFixedUpdate();

                if (Mathf.Abs(rigid.velocity.x) < 0.05f) break;
            }
            if (Mathf.Abs(rigid.velocity.x) < 0.05f) break;
        }
        anim.SetInteger("animNumber", 0);
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        power = 10f;
        state.ignoreKnockback = false;

        yield return new WaitForSeconds(2f);

        moveCoroutine = StartCoroutine(move0());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !state.isDead){
            PState = other.gameObject.GetComponent<PlayerState>();
            PState.damaged(power);
            PState.stun(0.3f);
            PState.knockback(transform.position, power, 3);
        }
    }

    bool PlayerCheck(float range){
        raymask = (1 << 7) + (1 << 3);
        hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), new Vector3(dir, 0, 0), range, raymask);

        return (hit.collider != null && hit.collider.CompareTag("Player")) ? true : false;        
    }
    bool IsMovable(){
        movingTargetX = transform.position.x + Random.Range(0.5f, 1.0f) * dir;
        movingDistanceX = Mathf.Abs(movingTargetX - transform.position.x);

        raymask = 1 << 7;
        hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), new Vector3(dir, 0, 0), movingDistanceX, raymask);
        if(hit.collider != null && hit.collider.CompareTag("block")) return false;
        hit = Physics2D.Raycast(new Vector3(movingTargetX, transform.position.y + 0.5f, 0), new Vector3(0, -1, 0), 1, raymask);
        if (hit.collider == null || !hit.collider.CompareTag("block")) return false;
        return true;
    }
}
