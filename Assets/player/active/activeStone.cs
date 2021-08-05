using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class activeStone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "block") {
            transform.parent.gameObject.GetComponent<EffectSystem>().squareFragmentEffect(10, new Color(100/256f, 209/256f, 212/256f, 1f) , 10, transform.position);
            Destroy(gameObject);
        }
    }
}