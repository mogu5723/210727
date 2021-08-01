using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MapArray{
    public GameObject[] Map;
}

public class CamearControl : MonoBehaviour
{
    public GameObject player; public Transform target;
    public float speed;
    public MapArray[] chapter; MapControl mCtrl;
    int[] mapCode = new int[2];
    float height; float width;
   
    private void Awake() {
        speed = 3f;
        mapCode[0] = 0;
        mapCode[1] = 0;
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }
    void Start()
    {
        UIBlack.color = new Color(0, 0, 0, 1f);
        chapter[mapCode[0]].Map[mapCode[1]].SetActive(true);
        mCtrl = chapter[mapCode[0]].Map[mapCode[1]].GetComponent<MapControl>();
        StartCoroutine(FadeIn());
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

     public void PlayerSpon(int mapCode0, int mapCode1, float x, float y){
         UIBlack.color = new Color(0, 0, 0, 1f);
         chapter[mapCode[0]].Map[mapCode[1]].SetActive(false);
         player.SetActive(false);

         mapCode[0] = mapCode0; mapCode[1] = mapCode1;
         mCtrl = chapter[mapCode[0]].Map[mapCode[1]].GetComponent<MapControl>();
         chapter[mapCode0].Map[mapCode1].SetActive(true);
         target.position = new Vector3(x, y, 0);
         player.SetActive(true);
         transform.position = new Vector3(x, y, -10f);
         StartCoroutine(FadeIn());
    }

    public Image UIBlack;
    IEnumerator FadeIn(){
        float count = 1f;
        while(count > 0){
            yield return null;
            count -= Time.deltaTime;
            UIBlack.color = new Color(0, 0, 0, count);
        }
    }
}
