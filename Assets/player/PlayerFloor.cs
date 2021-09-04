using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    PlayerControl ctrl;
    Rigidbody2D rigid;
    Animator anim;
    PlayerState state;

    public int blockCount;

    private void Awake() {
        ctrl = transform.parent.GetComponent<PlayerControl>();
        rigid = transform.parent.GetComponent<Rigidbody2D>();
        anim = transform.parent.GetComponent<Animator>();
        state = transform.parent.GetComponent<PlayerState>();
        
        state.stand = false;
        
        blockCount = 0;
    }

    private void OnEnable() {
        blockCount = 0;
        state.stand = false;
    }

        
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("block")){
            blockCount++;
            state.stand = true;
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
