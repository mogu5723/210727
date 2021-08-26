using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropSystem : MonoBehaviour
{
    public DataManagement dataManagement;
    EffectSystem effectSystem;
    public GameObject coin1, coin10, coin100;
    public Text goldText;
    
    private void Awake() {
        effectSystem = GetComponent<EffectSystem>();
        goldText.text = "0";
        dataManagement.gameData.goldAmount = 0;
    }

    public void coinDrop(int min, int max, Transform mobTrans){
        int amount = Random.Range(min, max+1);
        int count1 = amount % 10, count10 = (amount/10)%10, count100 = amount/100;
        
        Debug.Log(count1+" "+count10+" "+count100);
        for(int i = 0; i < count1; i++){
            GameObject temp = Instantiate(coin1, mobTrans.position, Quaternion.identity, transform);
            effectSystem.tempObjList.Add(temp);
            
            Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
            temp.transform.Translate(dir);
            temp.GetComponent<Rigidbody2D>().velocity = dir*10;
        }
    }

    public void getGold(int amount){
        dataManagement.gameData.goldAmount += amount;
        goldText.text = dataManagement.gameData.goldAmount.ToString();
    }
}
