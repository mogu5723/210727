using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class GameData
{
    //player
    public int mapCode0, mapCode1;
    public float spawnX, spawnY;

    public int[] objData;

    public int invenCount; public int[] invenItemCode; public int[] invenItemCount;

    public int goldAmount;
}
