using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{

    public string upgradeName;
    public float upgradeCurrentLevel;
    public float upgradeCurrentDamage;
    public float upgradeBaseCost;
    public int heroUnlockOrder;
    private int[] bonusLevels;
    private int heroCostMultiplier = -9; // Smaller number DECREASES COST

    void Start()
    {

        this.GetComponentInChildren<Button>().onClick.AddListener(BuyUpgrade);
        LoadUI();
        //LoadUpgrades();
    }

    public int damageBonus(bool getNext = false)
    {
        int multiplier = 1;
        for (int i = 0; i < bonusLevels.Length; i++)
        {
            if (this.upgradeCurrentLevel == bonusLevels[i] && getNext == false)
            {

                multiplier += 1;
            }
            else if (this.upgradeCurrentLevel + 1 >= bonusLevels[i] && getNext == true)
            {
                multiplier += 1;
            }
        }
        return multiplier;
    }
    
    public float upgradeCost()
    {
        if (this.upgradeName == "Player")
        {
            //Player Upgrade Cost
            return Mathf.Round(5 * (Mathf.Pow(1.062f, this.upgradeCurrentLevel + 1) - Mathf.Pow(1.062f, this.upgradeCurrentLevel)) / 0.062f);
        }
        else
        {
            //Hero Upgrade Cost
            return Mathf.Round(this.upgradeBaseCost * Mathf.Pow(1.082f, this.upgradeCurrentLevel) * (Mathf.Pow(1.082F, 1f) - 1) / 0.82f * (1 - heroCostMultiplier));
        }
    }
    public float upgradeIncreaseDamage()
    {
        if (this.upgradeName == "Player")
        {
            //Player Damage Increase
            return ((upgradeCurrentLevel + 1) * damageBonus(true)) - upgradeCurrentDamage ; // difference between current damage and what it would be +1 level
        }
        else
        {
            //Hero Damage Increase  
            
            return ((upgradeBaseCost / 10 * (1 - 23 / 1000 * Mathf.Pow(Mathf.Min(heroUnlockOrder, 34), Mathf.Min(heroUnlockOrder, 34))) * (upgradeCurrentLevel + 1) * damageBonus(true)) - upgradeCurrentDamage);
        }
    }

    // UI
    private Sprite upgradeIcon;
    private TextMeshProUGUI upgradeNameText;
    private TextMeshProUGUI upgradeCurrentLevelText;
    private TextMeshProUGUI upgradeCurrentDamageText;
    // Buy Button
    private TextMeshProUGUI upgradeCostText;
    private TextMeshProUGUI upgradeIncreaseDamageText;

    
    

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
        bonusLevels = new int[5] { 10, 30, 50, 70, 90 }; // Levels where an increase in dps multiplier is applied

        //Name
        upgradeNameText = this.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        //Level
        upgradeCurrentLevelText = this.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        //Current Damage
        upgradeCurrentDamageText = this.transform.Find("DamageText").GetComponent<TextMeshProUGUI>();

        LoadUpgrades(); // Load data if savefile exists

        // After loading upgrade data, then we should re-calculate the upgrade costs and nextDPS

        //BUTTON - Cost
        upgradeCostText = this.transform.Find("BuyBtn").Find("CostText").GetComponent<TextMeshProUGUI>();
        upgradeCostText.text = upgradeCost().ToString();
        //BUTTON - DPS
        upgradeIncreaseDamageText = this.transform.Find("BuyBtn").Find("DPSIncreaseText").GetComponent<TextMeshProUGUI>();
        upgradeIncreaseDamageText.text = "+ " + upgradeIncreaseDamage().ToString() + " DPS";
        //Debug.Log(upgradeName + " increase dps = " + upgradeIncreaseDamage());
        

        upgradeNameText.text = upgradeName;
        upgradeCurrentLevelText.text = "(LVL: " + upgradeCurrentLevel.ToString() + " )";
        if (upgradeName == "Player")
        {
            upgradeCurrentDamageText.text = "Damage: " + GameData.sessionData.playerData.tapDamage.ToString();
        }
        else
        {
            upgradeCurrentDamageText.text = "Damage: " + upgradeCurrentDamage.ToString();
        }

        syncStart = true;
    }

    private void SyncUI()
    {
        if (GameData.sessionData.playerData.gold - this.upgradeCost() >= 0)
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
        if (GameData.sessionData.playerData.gold - this.upgradeCost() >= 0)
        {
            GameData.sessionData.playerData.gold -= this.upgradeCost(); // Take money from player
            if (this.upgradeName == "Player")
            {
                //Player set damage
                GameData.sessionData.playerData.tapDamage += upgradeIncreaseDamage();   //Sets the root PLayer tap damage
                upgradeCurrentDamage += upgradeIncreaseDamage(); // Increase the damage
                GameData.sessionData.playerUpgrade.currentDamage = upgradeCurrentDamage; //Sets the upgrade save data

                upgradeCurrentDamageText.text = "Damage: " + GameData.sessionData.playerData.tapDamage.ToString();

            }
            else
            {
                //Hero Set Damage
                GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].currentDamage += upgradeIncreaseDamage();
                upgradeCurrentDamage = GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].currentDamage;
                upgradeCurrentDamageText.text = "Damage: " + upgradeCurrentDamage.ToString() ;
            }
            upgradeCurrentLevel += 1;
            if (upgradeName == "Player")
            {
                GameData.sessionData.playerUpgrade.currentLevel += 1; // saves upgrade damage
            }
            else
            {
                GameData.sessionData.heroUpgrades[heroUnlockOrder-1].currentLevel += 1; // saves upgrade damage
                Debug.Log(upgradeName + " level = " + GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].currentLevel);
            }
            
            upgradeCostText.text = upgradeCost().ToString(); // Sets the upgrade cost to a new updated cost value
            upgradeCurrentLevelText.text = "(LVL: " + upgradeCurrentLevel.ToString() + " )"; // update the level
            upgradeIncreaseDamageText.text = "+ " + upgradeIncreaseDamage().ToString() + " DPS"; ;
            

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
            if (GameData.sessionData.playerUpgrade.upgradeName != "")
            {
                //SAVED DATA - LOAD VALUES BACK INTO THIS CLASS              
                upgradeName = GameData.sessionData.playerUpgrade.upgradeName;
                upgradeCurrentLevel = GameData.sessionData.playerUpgrade.currentLevel;                      
                heroUnlockOrder = GameData.sessionData.playerUpgrade.heroUnlockOrder;
                upgradeBaseCost = GameData.sessionData.playerUpgrade.baseCost;
            }
            else
            {              
                GameData.sessionData.playerUpgrade.upgradeName = upgradeName;
                GameData.sessionData.playerUpgrade.currentLevel = 1; // 1 is default level for player
                GameData.sessionData.playerUpgrade.currentDamage = GameData.sessionData.playerData.tapDamage;
                GameData.sessionData.playerUpgrade.heroUnlockOrder = heroUnlockOrder;
                GameData.sessionData.playerUpgrade.baseCost = upgradeBaseCost;
            }
            upgradeCurrentDamage = GameData.sessionData.playerUpgrade.currentDamage;
        }
        else
        {
            if (GameData.sessionData.heroUpgrades[heroUnlockOrder-1].upgradeName != "")
            {
                Debug.Log("Save File for fluffers WAS found");
                upgradeName = GameData.sessionData.heroUpgrades[heroUnlockOrder-1].upgradeName;
                upgradeCurrentLevel = GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].currentLevel;
                heroUnlockOrder = GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].heroUnlockOrder;
                upgradeBaseCost = GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].baseCost;
            }
            else
            {
                Debug.Log(" NO Save File for flufferswas  found");
                GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].upgradeName = upgradeName;
                GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].currentLevel = upgradeCurrentLevel;
                GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].currentDamage = upgradeCurrentDamage;
                GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].heroUnlockOrder = heroUnlockOrder;
                GameData.sessionData.heroUpgrades[heroUnlockOrder - 1].baseCost = upgradeBaseCost;
            }
            upgradeCurrentDamage = GameData.sessionData.heroUpgrades[heroUnlockOrder-1].currentDamage;
        }

    }

    public void CheatGold()
    {
        GameData.sessionData.playerData.gold += 25;
    }
}
