using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl : MonoBehaviour
{
    Transform cameraTransform;
    float posX;

    private void Awake() {
        cameraTransform = transform.parent;
        
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

        while(cameraTransform.position.x - posX > 8)
            posX -= 16;
        
        while(cameraTransform.position.x - posX < -8) 
            posX += 16;

        transform.position = new Vector3(posX, 0, 10f);
    }
}
