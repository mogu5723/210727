using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{
    private void Awake() {
        gameObject.GetComponent<Light2D>().intensity = 0.7f;
    }
}
