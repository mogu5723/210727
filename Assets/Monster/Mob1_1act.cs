using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1_1act : MonoBehaviour
{
    public MonsterState state;

    private void Awake() {
        state = GetComponent<MonsterState>();
    }

    private void OnEnable() {
        stateSetting();
    }

    void stateSetting(){
        state.hp = state.maxHp = 30;
        state.deadState = false;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
