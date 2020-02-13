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
        "DPSIncrease", // [4]
        "LevelUpText" // [5]
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
        panelName = this.transform.parent.parent.parent.parent.name; // Name of the panel this upgrade resides in (heroupgrades,playerupgrades ect)
        this.GetComponentInChildren<Button>().onClick.AddListener(BuyUpgrade); // Buy Upgrade Button
        syncStart = true;
    }

    //Handles color change of buttons when we can afford them
    private void SyncUI()
    {
        RefreshUI();
        Image buyBtn = this.GetComponentInChildren<Button>().gameObject.GetComponent<Image>();
        if (GameData.sessionData.playerData.gold - calculations[0] >= 0)
        {
            buyBtn.color = new Color32(215, 147, 0, 255);
        }
        else
        {
            buyBtn.color = new Color32(113, 113, 113, 255);
        }  
    }

    private float[] calculations;
    private string panelName;
    public void RefreshUI()
    {
        calculations = UpgradeUtils.CalculateUpgrade(upgradeName, upgrade.currentLevel, baseCost, heroUnlockOrder, FindObjectOfType<GameManager>().buyAmountDic[panelName]);
        UIDic[uiNames[1]].text = "(LVL: " + RoundingUtils.GetShorthand(upgrade.currentLevel) + ")";
        UIDic[uiNames[2]].text = "Damage: " + RoundingUtils.GetShorthand(upgrade.currentDamage) + " DPS";
        UIDic[uiNames[3]].text = RoundingUtils.GetShorthand(Mathf.Round(calculations[0]));
        UIDic[uiNames[4]].text = "+ " + RoundingUtils.GetShorthand(Mathf.Round(calculations[1])) + " DPS";
        UIDic[uiNames[5]].text = "BUY: " + RoundingUtils.GetShorthand(Mathf.Round(calculations[2]));
    }

    private void BuyUpgrade()
    {
        if (GameData.sessionData.playerData.gold - calculations[0] >= 0)
        {
            GameData.sessionData.playerData.gold -= calculations[0];
            upgrade.currentDamage += calculations[1];
            upgrade.currentLevel += (int)calculations[2];
        }
    }

    public void CheatGold()
    {
        GameData.sessionData.playerData.gold += 81;
            //FindObjectOfType<Monster>().maxHP * 0.008f + 0.0002f * Mathf.Min(GameData.sessionData.playerData.stage, 150); // give gold dependant on stage
    }

    public void NextStage()
    {
        GameData.sessionData.playerData.stage += 5;
    }
}
