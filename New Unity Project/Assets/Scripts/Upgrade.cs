using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{

    public string upgradeName;
    public int heroUnlockOrder;
    public int currentLevel;
    public float upgradeCost;
    public float upgradeDPS;
    public float baseCost;
    public float currentDamage;

    // UI
    private TextMeshProUGUI upgradeNameText;
    private TextMeshProUGUI currentLevelText;
    private TextMeshProUGUI currentDamageText;
    // Buy Button
    private TextMeshProUGUI upgradeCostText;
    private TextMeshProUGUI upgradeDPSText;

    void Start()
    {
        LoadUI();
    }

    private bool syncStart = false;
    void Update()
    {
        if (syncStart == true)
        {
            SyncUI();
        }
    }

    private void LoadUI()
    {
        //Name
        upgradeNameText = this.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        //Level
        currentLevelText = this.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        //Current Damage
        currentDamageText = this.transform.Find("DamageText").GetComponent<TextMeshProUGUI>();
        //BUTTON - Cost
        upgradeCostText = this.transform.Find("BuyBtn").Find("CostText").GetComponent<TextMeshProUGUI>();
        //BUTTON - DPS
        upgradeDPSText = this.transform.Find("BuyBtn").Find("DPSIncreaseText").GetComponent<TextMeshProUGUI>();
        //BUTTON
        this.GetComponentInChildren<Button>().onClick.AddListener(BuyUpgrade);

        LoadUpgrades(); // Load data if savefile exists
        RefreshUI(); // Reload/Recalculate values
        syncStart = true;
    }

    //Handles color change of buttons when we can afford them
    private void SyncUI()
    {
        if (GameData.sessionData.playerData.gold - upgradeCost >= 0)
        {
            this.transform.Find("BuyBtn").GetComponent<Image>().color = new Color32(215, 147, 0, 255);
        }
        else
        {
            this.transform.Find("BuyBtn").GetComponent<Image>().color = new Color32(113, 113, 113, 255);
        }
    }

    //Will Re-calculate the upgrade costs and values, and will update this upgrade's UI
    private void RefreshUI()
    {
        float[] upgradeResults = UpgradeUtils.CalculateUpgrade(upgradeName, currentLevel, baseCost, heroUnlockOrder);
        upgradeCost = upgradeResults[0];
        upgradeDPS = upgradeResults[1];
        upgradeCostText.text = upgradeCost.ToString();
        upgradeDPSText.text = "+" + upgradeDPS.ToString() + " DPS";
        upgradeNameText.text = upgradeName;
        currentDamageText.text = "Damage: " + currentDamage.ToString() + " DPS";
        currentLevelText.text = "(LVL: " + currentLevel.ToString() + ")";
    }

    private void BuyUpgrade()
    {
        if (GameData.sessionData.playerData.gold - upgradeCost >= 0)
        {
            GameData.sessionData.playerData.gold -= upgradeCost; // Take money from player
            UpgradeData data;

            //Determines which upgrade we edit
            if (upgradeName == "Player")
            {
                data = GameData.sessionData.playerUpgrade;
                GameData.sessionData.playerData.tapDamage += upgradeDPS;   //Sets the root Player tap damage 
            }
            else
            {
                data = GameData.sessionData.heroUpgrades[heroUnlockOrder-1];          
            }

            currentLevel += 1; // Increase Level
            currentDamage += upgradeDPS;
            data.currentDamage = currentDamage;
            currentDamageText.text = "Damage: " + currentDamage + " DPS";
            data.currentLevel = currentLevel;

            RefreshUI();
        }
        else
        {
            Debug.Log("Cant afford");
        }
    }



    private void LoadUpgrades()
    {
        UpgradeData data;

        //Determines which upgrade we edit
        if (upgradeName == "Player")
        {
            data = GameData.sessionData.playerUpgrade;
            // Set these values here because this is not "unlockable" and is the default 
            currentDamage = GameData.sessionData.playerData.tapDamage;
            currentLevel = 1; 
        }
        else
        {
            data = GameData.sessionData.heroUpgrades[heroUnlockOrder-1];
        }


        if (data.upgradeName != "")
        {
            //SAVED DATA - LOAD VALUES BACK INTO THIS CLASS              
            upgradeName = data.upgradeName;
            currentLevel = data.currentLevel;
            heroUnlockOrder = data.heroUnlockOrder;
            baseCost = data.baseCost;
        }
        else
        {
            data.upgradeName = upgradeName;
            data.currentLevel = currentLevel;
            data.currentDamage = currentDamage;
            data.heroUnlockOrder = heroUnlockOrder;
            data.baseCost = baseCost;
        }

        currentDamage = data.currentDamage;
    }

    public void CheatGold()
    {
        GameData.sessionData.playerData.gold += 25;
    }
}
