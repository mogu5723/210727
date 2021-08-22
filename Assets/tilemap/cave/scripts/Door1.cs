using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : MonoBehaviour
{
    //데이터 세이브
    DataManagement dataManagement; GameData gameData;
    public int objNum; int stateNumber;


    Vector3 initailPos;
    float height;

    private void Awake() {
        dataManagement = transform.parent.parent.GetComponent<DataSetting>().dataManagement; //데이터 저장&불러오기
        initailPos = transform.position;

        height = 0;
    }

    private void OnEnable() {
        stateNumber = dataManagement.gameData.objData[objNum];
        if(stateNumber == 1){
            height = 2;
            transform.position = initailPos + new Vector3(0, 2f, 0);
        }else{
            height = 0;
            transform.position = initailPos;
        }
    }

    public void Open(){
        StartCoroutine(Open_());
    }
    IEnumerator Open_(){
        stateNumber = 1;
        dataManagement.gameData.objData[objNum] = stateNumber;

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
        stateNumber = 0;
        dataManagement.gameData.objData[objNum] = stateNumber;

        while(height > 0){
            transform.position -= new Vector3(0, Time.deltaTime, 0);
            height -= Time.deltaTime;
            yield return null;
        }
        transform.position -= new Vector3(0, height, 0);
    }
}
