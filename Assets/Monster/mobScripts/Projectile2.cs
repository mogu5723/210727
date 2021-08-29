using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    PlayerState PState;
    Vector3 velocity;

    private void Awake() {
        velocity = new Vector3(0, -9, 0);
    }
    
    private void FixedUpdate() {
        transform.Translate(velocity*0.02f);

        if(transform.position.y < -10)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            PState = other.GetComponent<PlayerState>();
            PState.stun(0.2f);
            PState.knockback(Vector3.zero, 0, 4);
            PState.damaged(20f);
            Destroy(gameObject);
        }
    }
}
