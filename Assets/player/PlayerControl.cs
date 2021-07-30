using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    SpriteRenderer rend;
    Rigidbody2D rigid;
    Animator anim;

    GameObject floor;

    float speed;
    public bool stand;

    private void Awake() {
        rend = gameObject.GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        floor = transform.Find("floor").gameObject;

        speed = 5f;
        jumpMotion = false;
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

        if(stand) rigid.velocity = new Vector2(moveDirection*speed, rigid.velocity.y);
        else {
            rigid.velocity += new Vector2(moveDirection*speed*Time.deltaTime*3, 0);
            
        }
        if(rigid.velocity.x > speed)
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        else if(rigid.velocity.x < -speed)
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
        if(rigid.velocity.y < -15)
            rigid.velocity = new Vector2(rigid.velocity.x, -15);

        if(moveDirection != 0 && stand) anim.SetInteger("animNumber", 1);
        else if(stand) anim.SetInteger("animNumber", 0);
    }
    bool jumpMotion;
    void EndJumpMotion(){
        jumpMotion = false;
    }
    void addJump(){
        if(Input.GetKey(KeyCode.C)){
            rigid.velocity += new Vector2(0, 3f);
        }
    }
    void JumpControl(){
        if(Input.GetKeyDown(KeyCode.C) && stand){
            stand = false;
            transform.Translate(new Vector3(0, 0.1f, 0));
            rigid.velocity = new Vector2(rigid.velocity.x, 15f);
            anim.SetInteger("animNumber", 2);
            anim.SetTrigger("change");
            jumpMotion = true;
        }
        if(!stand && !jumpMotion){
            anim.SetInteger("animNumber", 3);
        }
    }

    void Update()
    {
        MoveControl();
        JumpControl();
    }
}