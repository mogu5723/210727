using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1_3_light : MonoBehaviour
{
    PlayerState PState;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            PState = other.gameObject.GetComponent<PlayerState>();
            PState.damaged(10f);
            PState.stun(1f);
            PState.knockback(Vector3.zero, 0, 0);
        }
    }
}
