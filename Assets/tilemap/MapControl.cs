using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public Vector2 center;
    public Vector2 size;

    void Start()
    {
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
    
    void Update()
    {
        
    }
}
