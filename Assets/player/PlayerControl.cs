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

    GameObject floor;

    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        state = GetComponent<PlayerState>();
        interaction = GetComponent<Interaction>();

        floor = transform.Find("floor").gameObject;

        jumpMotion = false;

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

        if(moveDirection == 1) rend.flipX = false;
        else if(moveDirection == -1) rend.flipX = true;
        
        if(!state.stunState){
            rigid.velocity = new Vector2(moveDirection*state.speed, rigid.velocity.y);
            if(rigid.velocity.x > state.speed)
                rigid.velocity = new Vector2(state.speed, rigid.velocity.y);
            else if(rigid.velocity.x < -state.speed)
                rigid.velocity = new Vector2(-state.speed, rigid.velocity.y);
        }

        if(!state.mining){
            if((moveDirection != 0 && state.stand)) anim.SetInteger("animNumber", 1);
            else if(state.stand) anim.SetInteger("animNumber", 0);
        }
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
        if(Input.GetKeyDown(KeyCode.C) && state.stand && state.actionable()){
            state.stand = false;
            transform.Translate(new Vector3(0, 0.1f, 0));
            rigid.velocity = new Vector2(rigid.velocity.x, 15f);
            anim.SetInteger("animNumber", 2);
            anim.SetTrigger("change");
            jumpMotion = true;
        }
        if(!state.stand && !jumpMotion){
            anim.SetInteger("animNumber", 3);
        }
    }

    void Update()
    {
        MoveControl();
        JumpControl();
        Interact();
        slotSelect();
    }

    //interaction
    public List<GameObject> interactObj;

    void Interact(){
        if(Input.GetKeyDown(KeyCode.Space) && state.stand && state.actionable() && interactObj.Count > 0){
            GameObject obj = GetCloseObject();

            if(obj.name == "rock") StartCoroutine(interaction.mining(obj));
        }
    }
    GameObject GetCloseObject(){
        GameObject temp;
        float minDistance, distance;
        float posX;

        posX = interactObj[0].transform.position.x + interactObj[0].GetComponent<BoxCollider2D>().size.x/2f-0.5f;
        distance = minDistance = Mathf.Abs(transform.position.x - posX);
        temp = interactObj[0];
        foreach(GameObject obj in interactObj){
            posX = obj.transform.position.x + obj.GetComponent<BoxCollider2D>().size.x/2f-0.5f;
            distance = Mathf.Abs(transform.position.x - posX);
            if(minDistance > distance) minDistance = distance;
            temp = obj;
        }

        return temp;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "mineral"){
            interactObj.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "mineral"){
            interactObj.Remove(other.gameObject);
        }
    }
    

    //use item
    int slotSelectNumber, pastSlotNumber;
    void slotSelect(){
        pastSlotNumber = slotSelectNumber;
        if(Input.GetKeyDown(KeyCode.Alpha1)) slotSelectNumber = 1;
        else if(Input.GetKeyDown(KeyCode.Alpha2)) slotSelectNumber = 2;
        else if(Input.GetKeyDown(KeyCode.Alpha3)) slotSelectNumber = 3;
        else if(Input.GetKeyDown(KeyCode.Alpha4)) slotSelectNumber = 4;
        else if(Input.GetKeyDown(KeyCode.Alpha5)) slotSelectNumber = 5;
        else return;

        invenManager.quickslot[pastSlotNumber-1].bgImage.color = new Color(1f, 1f, 1f, 80/256f);
        invenManager.quickslot[slotSelectNumber-1].bgImage.color = new Color(251/256f, 255/256f, 0, 80/256f);
    }
}