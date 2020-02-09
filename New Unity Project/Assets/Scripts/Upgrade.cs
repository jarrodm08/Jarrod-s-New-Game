﻿using System.Collections;
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
    public float HeroUnlockOrder;
    private int[] bonusLevels;

    private int HeroCostMultiplier = -9; // Smaller number DECREASES COST
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
        Debug.Log("Multiplier: " + multiplier);
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
            return Mathf.Round(this.upgradeBaseCost * Mathf.Pow(1.082f, this.upgradeCurrentLevel) * (Mathf.Pow(1.082F, 1f) - 1) / 0.82f * (1 - HeroCostMultiplier));
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
            return (Mathf.Pow(upgradeBaseCost / 10 * (1 - 23 / 1000 * Mathf.Min(this.HeroUnlockOrder, 34)), Mathf.Min(HeroUnlockOrder, 34)) * this.upgradeCurrentLevel + 1) * damageBonus(true) - upgradeCurrentDamage;
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
                upgradeCurrentDamage += upgradeIncreaseDamage();
                upgradeCurrentDamageText.text = "Damage: " + this.upgradeCurrentDamage.ToString() ;
            }
            this.upgradeCurrentLevel += 1;
            GameData.sessionData.playerUpgrade.currentLevel += 1; // saves upgrade damage
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
                Debug.Log("Player Upgrade - Found Saved data: upgradeCurrentLevel = " + GameData.sessionData.playerUpgrade.currentLevel);
                upgradeName = GameData.sessionData.playerUpgrade.upgradeName;
                upgradeCurrentLevel = GameData.sessionData.playerUpgrade.currentLevel;              
               
                HeroUnlockOrder = GameData.sessionData.playerUpgrade.heroUnlockOrder;
                upgradeBaseCost = GameData.sessionData.playerUpgrade.baseCost;
            }
            else
            {
                Debug.Log("No Saved data for player Upgrade");
                GameData.sessionData.playerUpgrade.upgradeName = upgradeName;
                GameData.sessionData.playerUpgrade.currentLevel = 1; // 1 is default level for player
                GameData.sessionData.playerUpgrade.currentDamage = GameData.sessionData.playerData.tapDamage;
                GameData.sessionData.playerUpgrade.heroUnlockOrder = HeroUnlockOrder;
                GameData.sessionData.playerUpgrade.baseCost = upgradeBaseCost;
            }
            upgradeCurrentDamage = GameData.sessionData.playerUpgrade.currentDamage;
            Debug.Log("LOading player upgrade damage as: " + upgradeCurrentDamage);
        }

    }

    public void CheatGold()
    {
        GameData.sessionData.playerData.gold += 25;
    }
}
