using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    SpriteRenderer rend;
    Rigidbody2D rigid;
    Animator anim;

    PlayerState state; Interaction interaction;
    public InventoryManager invenManager;

    GameObject floor; GameObject Attack1Range;

    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        state = GetComponent<PlayerState>();
        interaction = GetComponent<Interaction>();

        floor = transform.Find("floor").gameObject;
        Attack1Range = transform.Find("attack1Range").gameObject;

        isJumping = false;

        transform.Find("Point Light 2D").GetComponent<Light2D>().intensity = 0.7f;

        slotSelectNumber = 1; pastSlotNumber = 1;
    }
    void Start()
    {
        
    }

    int moveDirection;
    void MoveControl(){
        moveDirection = 0;
        if(state.actionable()){
            if(Input.GetKey(KeyCode.LeftArrow)) moveDirection--;
            if(Input.GetKey(KeyCode.RightArrow)) moveDirection++;
        }

        if(moveDirection == 1 && !state.isAttacking) rend.flipX = false;
        else if(moveDirection == -1 && !state.isAttacking) rend.flipX = true;
        
        if(!state.stunState){
            rigid.velocity = new Vector2(moveDirection*state.speed, rigid.velocity.y);
            if(rigid.velocity.x > state.speed)
                rigid.velocity = new Vector2(state.speed, rigid.velocity.y);
            else if(rigid.velocity.x < -state.speed)
                rigid.velocity = new Vector2(-state.speed, rigid.velocity.y);
        }

        if(!state.isInteractive && !state.isAttacking){
            if((moveDirection != 0 && state.stand) && rigid.velocity.y < 1) anim.SetInteger("animNumber", 1);
            else if(state.stand && rigid.velocity.y < 1) anim.SetInteger("animNumber", 0);
        }
    }
    bool isJumping;
    void EndJumpMotion(){
        isJumping = false;
    }
    void addJump(){
        if(Input.GetKey(KeyCode.C)){
            rigid.velocity += new Vector2(0, 3f);
        }
    }
    void JumpControl(){
        if(!state.stand && !isJumping && !state.isAttacking){
            anim.SetInteger("animNumber", 3);
        }
        if(Input.GetKeyDown(KeyCode.C) && state.stand && state.actionable()){
            transform.Translate(new Vector3(0, 0.1f, 0));
            rigid.velocity = new Vector2(rigid.velocity.x, 15f);
            if(!state.isAttacking){
                anim.SetInteger("animNumber", 2);
                anim.SetTrigger("change");
                isJumping = true;
            }
        }
    }

    void Update()
    {
        MoveControl();
        JumpControl();
        interaction.Interact();
        interaction.PickUp();
        slotSelect();
        AttackControl();
    }

    //attack
    void AttackControl(){
        if(state.actionable() && Input.GetKeyDown(KeyCode.A) && !state.isAttacking){
            StartCoroutine(attack1());
        }
    }
    
    IEnumerator attack1(){
        state.isAttacking = true;
        state.attackDir = rend.flipX ? -1 : 1;
        anim.SetInteger("animNumber", 5);

        Attack1Range.SetActive(true);
        yield return new WaitForSeconds(0.4166f);
        state.isAttacking = false;
        anim.SetInteger("animNumber", 3);
    }



    //use item
    public int slotSelectNumber, pastSlotNumber;
    void slotSelect(){
        pastSlotNumber = slotSelectNumber;
        if(Input.GetKeyDown(KeyCode.Alpha1)) slotSelectNumber = 1;
        else if(Input.GetKeyDown(KeyCode.Alpha2)) slotSelectNumber = 2;
        else if(Input.GetKeyDown(KeyCode.Alpha3)) slotSelectNumber = 3;
        else if(Input.GetKeyDown(KeyCode.Alpha4)) slotSelectNumber = 4;
        else if(Input.GetKeyDown(KeyCode.Alpha5)) slotSelectNumber = 5;
        else return;

        invenManager.quickslot[pastSlotNumber-1].bgImage.color = new Color(1f, 1f, 1f, 80/256f);
        invenManager.quickslot[slotSelectNumber-1].bgImage.color = new Color(1f, 1f, 1f, 160/256f);
    }
}