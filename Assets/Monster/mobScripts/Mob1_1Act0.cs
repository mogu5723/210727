using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1_1Act0 : MonoBehaviour
{
    PlayerState PState; 

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("block")) {
            transform.parent.gameObject.GetComponent<EffectSystem>().squareFragmentEffect(10, new Color(100/256f, 209/256f, 212/256f, 1f) , 10, transform.position);
            Destroy(gameObject);
        }else if(other.CompareTag("Player")){
            transform.parent.gameObject.GetComponent<EffectSystem>().squareFragmentEffect(10, new Color(100/256f, 209/256f, 212/256f, 1f) , 10, transform.position);
            
            PState = other.GetComponent<PlayerState>();
            
            PState.stun(0.2f);
            PState.knockback(GetComponent<Rigidbody2D>().velocity, 5, 2);
            PState.damaged(10f);

            Destroy(gameObject);
        }
    }
}
