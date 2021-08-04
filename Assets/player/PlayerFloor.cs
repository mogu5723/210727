using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    PlayerControl ctrl;
    Rigidbody2D rigid;
    Animator anim;
    PlayerState state;

    float fallDamage;

    private void Awake() {
        ctrl = transform.parent.GetComponent<PlayerControl>();
        rigid = transform.parent.GetComponent<Rigidbody2D>();
        anim = transform.parent.GetComponent<Animator>();
        state = transform.parent.GetComponent<PlayerState>();
        
        state.stand = false;

        fallDamage = 0;
    }

    private void Update() {
        if(rigid.velocity.y < -40){
            fallDamage = -rigid.velocity.y - 39; 
        }
    }
        
    private void OnTriggerEnter2D(Collider2D other) {
        if(fallDamage > 0){
            state.damaged(fallDamage);
            state.stun(0.5f);
            StartCoroutine(state.knockback(Vector3.zero, 0, 0));
            fallDamage = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "block"){
            state.stand = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "block"){
            state.stand = false;
        }
    }
}
