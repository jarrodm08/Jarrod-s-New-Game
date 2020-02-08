using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Upgrade
{
    public string upgradeName;
    public float currentLevel;
    public float upgradeIndex;

   public Upgrade()
    {
        upgradeName = "";
        currentLevel = 0;
    }
}
