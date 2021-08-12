using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour
{
    private void OnEnable() {
        transform.Find("Canvas").Find("hpbar").gameObject.SetActive(false);
    }
}
