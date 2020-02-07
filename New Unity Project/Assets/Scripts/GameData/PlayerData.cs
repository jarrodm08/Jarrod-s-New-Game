using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float gold;
    public float stage;
    public float monsterNum;
    public float tapDamage;

    public PlayerData()
    {
        //Set Defaults Here
        gold = 0f;
        stage = 1f;
        monsterNum = 1f;
        tapDamage = 12f;
    }
}