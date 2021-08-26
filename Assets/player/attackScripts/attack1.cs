using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack1 : MonoBehaviour
{
    public PlayerState state;
    MonsterState monsterState;
    BoxCollider2D col;

    List<GameObject> attackedList;

    private void Awake() {
        col = GetComponent<BoxCollider2D>();

        attackedList = new List<GameObject>();
    }

    private void OnEnable() {
        col.offset =  new Vector2(0.5f*state.attackDir, 0.34375f);
        col.size = new Vector2(0f, 0.1875f);
        attackedList.Clear();
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
            if(attackedList.Contains(other.gameObject)) return;
            attackedList.Add(other.gameObject);

            monsterState = other.gameObject.GetComponent<MonsterState>();
            monsterState.stun(0.1f);
            monsterState.knockback(new Vector3(state.attackDir, 0, 0), 5f, 2);
            monsterState.damaged(10f);

            state.stun(0.2f);
            state.knockback(new Vector3(-state.attackDir, 0, 0), 3f, 4);
        }
    }
}
