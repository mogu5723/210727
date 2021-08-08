using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class activeStone : MonoBehaviour
{
    MonsterState MState;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("block")) {
            transform.parent.gameObject.GetComponent<EffectSystem>().squareFragmentEffect(10, new Color(100/256f, 209/256f, 212/256f, 1f) , 10, transform.position);
            Destroy(gameObject);
        }else if(other.CompareTag("monster")){
            transform.parent.gameObject.GetComponent<EffectSystem>().squareFragmentEffect(10, new Color(100/256f, 209/256f, 212/256f, 1f) , 10, transform.position);
            Destroy(gameObject);
            
            MState = other.GetComponent<MonsterState>();
            
            MState.stun(0.2f);
            MState.knockback(GetComponent<Rigidbody2D>().velocity, 5, 2);
            MState.damaged(10f);
        }
    }
}
