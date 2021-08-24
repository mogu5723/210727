using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl : MonoBehaviour
{
    public Transform cameraTransform;
    float posX;

    private void Awake() {
        posX = 0f;
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void LateUpdate() {
        posX = cameraTransform.position.x*0.8f;

        while(cameraTransform.position.x - posX > 16)
            posX += 32;
        
        while(cameraTransform.position.x - posX < -16) 
            posX -= 32;

        transform.position = new Vector3(posX, cameraTransform.position.y, 10f);
    }
}
