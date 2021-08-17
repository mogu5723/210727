using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : MonoBehaviour
{
    float height;

    private void Awake() {
        height = 0;
    }

    public void Open(){
        StartCoroutine(Open_());
    }
    IEnumerator Open_(){
        while(height < 2){
            transform.position += new Vector3(0, Time.deltaTime, 0);
            height += Time.deltaTime;
            yield return null;
        }
        transform.position -= new Vector3(0, height - 2f, 0);
    }

    public void Close(){
        StartCoroutine(Close_());
    }
    IEnumerator Close_(){
        while(height > 0){
            transform.position -= new Vector3(0, Time.deltaTime, 0);
            height -= Time.deltaTime;
            yield return null;
        }
        transform.position -= new Vector3(0, height, 0);
    }
}
