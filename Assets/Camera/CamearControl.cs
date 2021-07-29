using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapArray{
    public GameObject[] Map;
}

public class CamearControl : MonoBehaviour
{
    public Transform target;
    float speed;
    public MapArray[] chapter;
    MapControl mCtrl;
    int[] mapCode = new int[2];
    float height;
    float width;
   
    private void Awake() {
        speed = 3f;
        mapCode[0] = 0;
        mapCode[1] = 0;
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }
    void Start()
    {
        chapter[mapCode[0]].Map[mapCode[1]].SetActive(true);
        mCtrl = chapter[mapCode[0]].Map[mapCode[1]].GetComponent<MapControl>();
    }

    float lx, ly;
    float clampX, clampY;
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

        lx = mCtrl.size.x * 0.5f - width;
        clampX = Mathf.Clamp(transform.position.x, -lx + mCtrl.center.x, lx + mCtrl.center.x);
        ly = mCtrl.size.y * 0.5f - height;
        clampY= Mathf.Clamp(transform.position.y, -ly + mCtrl.center.y, ly + mCtrl.center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }
}
