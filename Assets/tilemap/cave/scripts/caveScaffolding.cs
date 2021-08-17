using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class caveScaffolding : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer rend;

    public UnityEvent On, Off;

    public int count;

    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
        count = 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") || other.CompareTag("monster") || other.name == "movableStone"){
            count++;
            if(count == 1) {
                On.Invoke();
                rend.sprite = sprites[1];
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player") || other.CompareTag("monster") || other.name == "movableStone"){
            count--;
            if(count == 0) {
                Off.Invoke();
                rend.sprite = sprites[0];
            }
        }
    }
}
