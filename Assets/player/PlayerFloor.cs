using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    PlayerControl ctrl;
    Animator anim;

    private void Awake() {
        ctrl = transform.parent.GetComponent<PlayerControl>();
        anim = transform.parent.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "block"){
            ctrl.stand = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "block"){
            ctrl.stand = false;
        }
    }
}
