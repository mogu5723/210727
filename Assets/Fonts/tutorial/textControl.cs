using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textControl : MonoBehaviour
{
    public GameObject player;
    TextMesh text;

    float distance;

    private void Awake() {
        text = gameObject.GetComponent<TextMesh>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {   
        distance = Vector3.Distance(player.transform.position,transform.position);
        if(distance > 6){
            text.color = new Color(0f, 0f, 0f, 0);
        }else if(distance < 4){
            text.color = new Color(0f, 0f, 0f, 1f);
        }else{
            text.color = new Color(0f, 0f, 0f, (6-distance)/2f);
        }
    }
}
