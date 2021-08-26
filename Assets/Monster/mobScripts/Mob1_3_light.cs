using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1_3_light : MonoBehaviour
{
    DataSetting dataSetting;
    PlayerState PState;

    private void Awake() {
        dataSetting = transform.parent.parent.parent.GetComponent<DataSetting>();
        PState = dataSetting.playerState;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player")){
            PState.stun(1f);
            PState.knockback(Vector3.zero, 0, 4);
            PState.damaged(10f);
        }
    }
}
