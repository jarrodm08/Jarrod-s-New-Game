using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeData
{
    public int currentLevel;
    public float currentDamage;

    public bool unlocked;
    public UpgradeData()
    {
        unlocked = false;        
    }
}
