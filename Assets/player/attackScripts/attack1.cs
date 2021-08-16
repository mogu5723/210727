using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack1 : MonoBehaviour
{
    public PlayerState state;
    MonsterState monsterState;
    BoxCollider2D col;

    private void Awake() {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnEnable() {
        col.offset =  new Vector2(0.5f*state.attackDir, 0.34375f);
        col.size = new Vector2(0f, 0.1875f);
        StartCoroutine(attack());
    }   

    IEnumerator attack(){
        for(int i = 0; i < 5; i++){
            col.offset += new Vector2(0.2f*state.attackDir, 0);
            col.size += new Vector2(0.4f, 0);
            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("monster")){
            monsterState = other.gameObject.GetComponent<MonsterState>();
            monsterState.stun(0.2f);
            monsterState.knockback(new Vector3(state.attackDir, 0, 0), 5f, 2);
            monsterState.damaged(10f);
        }
    }
}
