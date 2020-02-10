using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeData
{

    public string upgradeName;
    public int currentLevel;
    public int heroUnlockOrder;
    public int upgradeIndex;
    public float currentDamage;
    public float baseCost;

    public bool unlocked;
    public UpgradeData()
    {
        upgradeName = "";
        unlocked = false;
    }
}
