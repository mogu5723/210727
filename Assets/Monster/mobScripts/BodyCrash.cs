using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCrash : MonoBehaviour
{
    PlayerState PState;
    MonsterState state;

    private void Awake() {
        state = transform.parent.GetComponent<MonsterState>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !state.isDead){
            PState = other.gameObject.GetComponent<PlayerState>();
            PState.damaged(10f);
            PState.stun(0.3f);
            PState.knockback(transform.position, 10f, 3);
        }
    }
}
