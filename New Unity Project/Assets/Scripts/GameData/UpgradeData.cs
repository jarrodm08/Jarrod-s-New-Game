using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public float currentLevel;
    public float currentDamage;
    public int heroUnlockOrder;
    public float baseCost;
    public float upgradeIndex;

   public UpgradeData()
    {
        upgradeName = "";
        currentLevel = 0;
    }
}
