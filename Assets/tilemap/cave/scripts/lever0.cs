using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class lever0 : MonoBehaviour
{
    DataManagement dataManagement; GameData gameData;
    public Sprite[] sprites;
    SpriteRenderer rend;
    public UnityEvent On, Off;

    int stateNumber;

    public int objNum;

    private void Awake() {
        dataManagement = transform.parent.parent.GetComponent<DataSetting>().dataManagement;
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        stateNumber = dataManagement.gameData.objData[objNum];
        rend.flipX = (stateNumber == 1 ? true : false);
    }

    public void Work(){
        if(stateNumber == 0){
            stateNumber = 1;
            rend.flipX = true;
            On.Invoke();
        }else if(stateNumber == 1){
            stateNumber = 0;
            rend.flipX = false;
            Off.Invoke();
        }

        dataManagement.gameData.objData[objNum] = stateNumber;
    }
}
