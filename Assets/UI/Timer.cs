using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{    
    TextMeshProUGUI textGUI;
    float time;
    private void Awake() {
        textGUI = GetComponent<TextMeshProUGUI>();

        textGUI.text = 0.00+"";
        time = 0;
    }
    void Update(){
        time += Time.deltaTime;
        textGUI.text = string.Format("{0:0.##}", time);

        if(Input.GetKeyDown(KeyCode.Escape)){
            Time.timeScale = 0;
        }
    }
}
