using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1_4Laser0 : MonoBehaviour
{
    PlayerState PState;

    private void Start() {
        PState = transform.parent.GetComponent<Mob1_4AI>().state.dataSetting.playerState;
    }

    

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player")){
            PState.stun(1f);
            PState.knockback(Vector3.zero, 0, 4);
            PState.damaged(10f);
        }
    }
}
