using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{
    public UpgradeData upgrade;
    public string upgradeName;
    public int heroUnlockOrder;
    public float baseCost;

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

    private Dictionary<string, TextMeshProUGUI> UIDic = new Dictionary<string, TextMeshProUGUI>();
    private List<string> uiNames = new List<string>()
    {
        "Name", // [0]
        "Level", // [1]
        "Damage", // [2]
        
        "Cost", // [3]
        "DPSIncrease" // [4]
    };

    private void LoadUI()
    {
        TextMeshProUGUI[] texts = this.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < texts.Length; i++)
        { // For each TextMeshProUGUI Element 
            for (int ii = 0; ii < uiNames.Count; ii++)
            {// check if it matches each of the uinames we have
                if (texts[i].name == uiNames[ii])
                {
                    UIDic.Add(uiNames[ii],texts[i]);
                }
            }
        }

        if ( upgradeName == "Player"){upgrade = GameData.sessionData.playerUpgrade; }
        else{ upgrade = GameData.sessionData.heroUpgrades[heroUnlockOrder-1]; }

        UIDic[uiNames[0]].text = upgradeName; // Set name
        this.GetComponentInChildren<Button>().onClick.AddListener(BuyUpgrade); // Buy Upgrade Button
        RefreshUI();
        syncStart = true;

    }

    //Handles color change of buttons when we can afford them
    private void SyncUI()
    {
        Image buyBtn = this.GetComponentInChildren<Button>().gameObject.GetComponent<Image>();
        if (GameData.sessionData.playerData.gold - upgradeCost()  >= 0)
        {
            buyBtn.color = new Color32(215, 147, 0, 255);
        }
        else
        {
            buyBtn.color = new Color32(113, 113, 113, 255);
        }
        RefreshUI();
    }

    #region Calculations
    private float upgradeCost()
    {
        return UpgradeUtils.CalculateUpgrade(upgradeName, upgrade.currentLevel, baseCost, heroUnlockOrder, FindObjectOfType<GameManager>().buyAmountDic["HeroUpgrades"])[0];
    }
    private float upgradeDPS()
    {
        return UpgradeUtils.CalculateUpgrade(upgradeName, upgrade.currentLevel, baseCost, heroUnlockOrder, FindObjectOfType<GameManager>().buyAmountDic["HeroUpgrades"])[1];
    }
    private int upgradeLevels()
    {
        return (int)UpgradeUtils.CalculateUpgrade(upgradeName, upgrade.currentLevel, baseCost, heroUnlockOrder, FindObjectOfType<GameManager>().buyAmountDic["HeroUpgrades"])[2];
    }
    #endregion

    private void RefreshUI()
    {   
        UIDic[uiNames[1]].text = "(LVL: " + RoundingUtils.GetShorthand(upgrade.currentLevel) + ")";
        UIDic[uiNames[2]].text = "Damage: " + RoundingUtils.GetShorthand(upgrade.currentDamage) + " DPS";
        UIDic[uiNames[3]].text = RoundingUtils.GetShorthand(upgradeCost());
        UIDic[uiNames[4]].text = " + " + RoundingUtils.GetShorthand(upgradeDPS()) + " DPS";
    }

    private void BuyUpgrade()
    {
        if (GameData.sessionData.playerData.gold - upgradeCost() >= 0)
        {
            GameData.sessionData.playerData.gold -= upgradeCost();
            upgrade.currentDamage += upgradeDPS();
            upgrade.currentLevel += upgradeLevels();
            RefreshUI();
        }
    }

    public void CheatGold()
    {
        GameData.sessionData.playerData.gold += Mathf.Ceil(FindObjectOfType<Monster>().maxHP * 0.008f + 0.0002f * Mathf.Min(GameData.sessionData.playerData.stage, 150)); // give gold dependant on stage
    }

    public void NextStage()
    {
        GameData.sessionData.playerData.stage += 5;
    }
}
