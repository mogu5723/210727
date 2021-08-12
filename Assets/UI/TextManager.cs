using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public List<GameObject> textList;

    private void Awake() {
        textList = new List<GameObject>();
    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void deleteText(){
        int count = textList.Count;
        for(int i = 0; i < count; i++){
            Destroy(textList[0]);
            textList.RemoveAt(0);
        }
    }
}
