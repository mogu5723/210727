using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class activeStone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "block") {
            transform.parent.gameObject.GetComponent<EffectSystem>().squareFragmentEffect(10, new Color(1f,1f,1f,1f), 10, transform.position);
            Destroy(gameObject);
        }
    }
}
