using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    SpriteRenderer rend;
    Rigidbody2D rigid;
    Animator anim;

    float speed;

    private void Awake() {
        rend = gameObject.GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        speed = 5f;
    }
    void Start()
    {
        
    }

    int moveDirection;
    void MoveControl(){
        moveDirection = 0;
        if(Input.GetKey(KeyCode.LeftArrow)) moveDirection--;
        if(Input.GetKey(KeyCode.RightArrow)) moveDirection++;

        if(moveDirection == 1) rend.flipX = false;
        else if(moveDirection == -1) rend.flipX = true;

        rigid.velocity = new Vector2(moveDirection*speed, 0);

        if(moveDirection != 0) anim.SetInteger("animNumber", 1);
        else anim.SetInteger("animNumber", 0);
    }
    void Update()
    {
        MoveControl();
    }
}
