using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    PlayerControl ctrl;
    Rigidbody2D rigid;
    Animator anim;
    PlayerState state;

    float fallDamage; public int blockCount;

    private void Awake() {
        ctrl = transform.parent.GetComponent<PlayerControl>();
        rigid = transform.parent.GetComponent<Rigidbody2D>();
        anim = transform.parent.GetComponent<Animator>();
        state = transform.parent.GetComponent<PlayerState>();
        
        state.stand = false;
        
        fallDamage = 0;
        blockCount = 0;
    }

    private void OnEnable() {
        blockCount = 0;
        state.stand = false;
    }

    private void FixedUpdate() {
        if(rigid.velocity.y < -30){
            fallDamage = -rigid.velocity.y - 29; 
        }
    }
        
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("block")){
            blockCount++;
            state.stand = true;
            if (fallDamage > 0){
                state.damaged(fallDamage);
                state.stun(0.5f);
                state.knockback(Vector3.zero, 0, 0);
                fallDamage = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("block")){
            blockCount--;
            if(blockCount == 0)
                state.stand = false;
        }
    }
}
