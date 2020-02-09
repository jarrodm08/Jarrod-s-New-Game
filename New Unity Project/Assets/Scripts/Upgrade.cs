using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{

    public string upgradeName;
    public int currentLevel;
    public float currentDamage;
    public float upgradeCost;
    public float upgradeDPS;
    public float baseCost;
    public int heroUnlockOrder;

    void Start()
    {
        LoadUI();
    }

    
    
    
    //public float upgradeIncreaseDamage()
    //{
    //    if (this.upgradeName == "Player")
    //    {
    //        //Player Damage Increase
    //        return ((upgradeCurrentLevel + 1) * GameData.sessionData.playerUpgrade.improvementBonus(true)) - upgradeCurrentDamage ; // difference between current damage and what it would be +1 level
    //    }
    //    else
    //    {
    //        //Hero Damage Increase  
            
    //        return ((upgradeBaseCost / 10 * (1 - 23 / 1000 * Mathf.Pow(Mathf.Min(heroUnlockOrder, 34), Mathf.Min(heroUnlockOrder, 34))) * (upgradeCurrentLevel + 1) * GameData.sessionData.heroUpgrades[heroUnlockOrder-1].improvementBonus(true)) - upgradeCurrentDamage);
    //    }
    //}

    // UI

    private TextMeshProUGUI upgradeNameText;
    private TextMeshProUGUI currentLevelText;
    private TextMeshProUGUI currentDamageText;
    // Buy Button
    private TextMeshProUGUI upgradeCostText;
    private TextMeshProUGUI upgradeDPSText;

    
    

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
        this.GetComponentInChildren<Button>().onClick.AddListener(BuyUpgrade);
        //BUTTON - DPS
        upgradeDPSText = this.transform.Find("BuyBtn").Find("DPSIncreaseText").GetComponent<TextMeshProUGUI>();


        LoadUpgrades(); // Load data if savefile exists
        // After loading upgrade data, then we should re-calculate the upgrade costs and nextDPS

        Debug.Log(upgradeName+currentLevel+baseCost);
        float[] upgradeResults = UpgradeUtils.CalculateUpgrade(upgradeName,currentLevel,baseCost,heroUnlockOrder);
        upgradeCost = upgradeResults[0];
        upgradeDPS = upgradeResults[1];
        upgradeCostText.text = upgradeCost.ToString();
        upgradeDPSText.text = "+" + upgradeDPS.ToString() + " DPS";
        upgradeNameText.text = upgradeName;
        currentDamageText.text = "Damage: " + currentDamage.ToString() + " DPS";
        currentLevelText.text = "(LVL: " + currentLevel.ToString() + ")";


        syncStart = true;
    }

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

    private void BuyUpgrade()
    {
        if (GameData.sessionData.playerData.gold - upgradeCost >= 0)
        {
            GameData.sessionData.playerData.gold -= upgradeCost; // Take money from player
            currentLevel += 1; // Increase Level
            UpgradeData data;

            if (upgradeName == "Player")
            {
                data = GameData.sessionData.playerUpgrade;
                GameData.sessionData.playerData.tapDamage += upgradeDPS;   //Sets the root Player tap damage 
            }
            else
            {
                data = GameData.sessionData.heroUpgrades[heroUnlockOrder-1];          
            }

            currentDamage += upgradeDPS;
            data.currentDamage = currentDamage;
            currentDamageText.text = "Damage: " + currentDamage + " DPS";
            data.currentLevel = currentLevel;

            float[] upgradeResults = UpgradeUtils.CalculateUpgrade(upgradeName, currentLevel, baseCost,heroUnlockOrder);
            upgradeCost = upgradeResults[0];
            upgradeDPS = upgradeResults[1];
            upgradeCostText.text = upgradeCost.ToString();
            upgradeDPSText.text = "+" + upgradeDPS.ToString() + " DPS";
            upgradeNameText.text = upgradeName;
            currentDamageText.text = "Damage: " + currentDamage.ToString() + " DPS";
            currentLevelText.text = "(LVL: " + currentLevel.ToString() + ")";
            //    GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].currentLevel += 1; // saves upgrade damage
        }
        else
        {
            Debug.Log("Cant afford");
        }
    }



    private void LoadUpgrades()
    {
        if (upgradeName == "Player")
        {
            UpgradeData data = GameData.sessionData.playerUpgrade;
            if (GameData.sessionData.playerUpgrade.upgradeName != "")
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
                data.currentLevel = 1; // 1 is default level for player
                currentLevel = data.currentLevel;
                data.currentDamage = GameData.sessionData.playerData.tapDamage;
                data.heroUnlockOrder = heroUnlockOrder;
                data.baseCost = baseCost;
            }

            currentDamage = data.currentDamage;
        }
        else
        {
            UpgradeData data = GameData.sessionData.heroUpgrades[heroUnlockOrder-1];
            if (data.upgradeName != "")
            {
                Debug.Log("Loading: " + upgradeName + " From SaveFile");
                upgradeName = data.upgradeName;
                currentLevel = data.currentLevel;
                heroUnlockOrder = data.heroUnlockOrder;
                baseCost = data.baseCost;
            }
            else
            {
                Debug.Log(upgradeName + " has no save data");
                data.upgradeName = upgradeName;
                data.currentLevel = currentLevel; // 0 is default level for hero
                data.currentDamage = currentDamage;
                data.heroUnlockOrder = heroUnlockOrder;
                data.baseCost = baseCost;
            }
            currentDamage = data.currentDamage;
        }

    }

    public void CheatGold()
    {
        GameData.sessionData.playerData.gold += 25;
    }
}
