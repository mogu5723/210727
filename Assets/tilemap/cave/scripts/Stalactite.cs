using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{   
    Rigidbody2D rigid;
    public float sponX, sponY;
    RaycastHit2D hit;

    int state; //0 = stop, 1 = drop, 2 = conflict

    private void Awake() {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        sponX = transform.position.x;
        sponY = transform.position.y;
    }

    private void OnEnable() {
        transform.position = new Vector3(sponX, sponY, 0);
        gameObject.layer = 0;
        rigid.gravityScale = 0;
        state = 0;
        transform.GetComponent<PolygonCollider2D>().isTrigger = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state == 0){
            for(int i = -1; i < 2; i++){
                hit = Physics2D.Raycast(transform.position+new Vector3(i, -0.6f, 0), new Vector3(0, -1f, 0), 7f, 136);
                if(hit.collider != null && hit.collider.tag == "Player"){
                    rigid.gravityScale = 8;
                    state = 1;
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(state == 1 && other.collider.tag == "Player"){
            PlayerState pst = other.transform.GetComponent<PlayerState>();

            pst.damaged(5f);
            pst.stun(0.3f);
            StartCoroutine(pst.knockback(transform.position, 10, 3));

            gameObject.layer = 6;
        }else if(state == 1 && other.collider.tag == "block"){
            state = 2;
            gameObject.layer = 6;
        }
    }
}
