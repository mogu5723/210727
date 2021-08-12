using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSetting : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraObject;
    CamearControl CCtrl;
    MapManager mapManager;

    public int[] mapCode;
    public float sponPointX, sponPointY;
    
    private void Awake() {
        CCtrl = cameraObject.GetComponent<CamearControl>();
        mapManager = transform.parent.parent.GetComponent<MapManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "Player"){
            CCtrl.PlayerSpawn(mapCode[0], mapCode[1], sponPointX, sponPointY);
        }
    }
}
