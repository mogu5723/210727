using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCrash : MonoBehaviour
{
    DataSetting dataSetting;
    PlayerState PState;
    MonsterState state;

    private void Awake() {
        state = transform.parent.GetComponent<MonsterState>();
        dataSetting = transform.parent.parent.parent.GetComponent<DataSetting>();
        PState = dataSetting.playerState;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player") && !state.isDead){
            PState.stun(0.3f);
            PState.knockback(transform.position, 5f, 1);
            PState.damaged(state.collisionDamage);
        }
    }
}
