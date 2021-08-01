using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{   
    Rigidbody2D rigid;
    public float sponX, sponY;
    RaycastHit2D hit;

    private void Awake() {
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        transform.position = new Vector3(sponX, sponY, 0);
        rigid.gravityScale = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position+new Vector3(0, -0.6f, 0), new Vector3(0, -1f, 0), 7f);
        if(hit.collider != null && hit.collider.tag == "Player"){
            rigid.gravityScale = 8;
        }
    }
}
