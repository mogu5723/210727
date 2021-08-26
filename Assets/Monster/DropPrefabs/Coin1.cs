using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin1 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform.CompareTag("Player")){
            PlayerState PState = other.transform.GetComponent<PlayerState>();
            transform.parent.GetComponent<DropSystem>().getGold(1);

            Destroy(gameObject);
        }
    }
}
