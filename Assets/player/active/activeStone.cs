using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class activeStone : MonoBehaviour
{
    MonsterState MState;
    Rigidbody2D rigid;
    EffectSystem effectSystem;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        effectSystem = transform.parent.gameObject.GetComponent<EffectSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("block")) {
            effectSystem.squareFragmentEffect(10, new Color(100/256f, 209/256f, 212/256f, 1f) , 10, transform.position);
            Destroy(gameObject);
        }else if(other.CompareTag("monster")){
            effectSystem.squareFragmentEffect(10, new Color(100/256f, 209/256f, 212/256f, 1f) , 10, transform.position);
            Destroy(gameObject);
            
            MState = other.GetComponent<MonsterState>();
            
            MState.stun(0.2f);
            MState.knockback(rigid.velocity, 5, 2);
            MState.damaged(10f);
        }
    }
}
