using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpgradePanel : MonoBehaviour
{
    private Transform content;
    private GameObject upgradePrefab;

    void Start()
    {
        content = this.transform.Find("Scroll View").Find("Viewport").Find("Content");
        upgradePrefab = Resources.Load("Prefabs/Upgrade") as GameObject;

        LoadUpgrades();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadUpgrades()
    {
        if (GameData.sessionData.playerUpgrades.Count == 0)
        {
            Debug.Log("No Upgrades Saved, Create New Upgrades now");
            GameData.sessionData.playerUpgrades.Add(new Upgrade() {upgradeName = "Player",currentLevel = 1});
            GameData.sessionData.playerUpgrades.Add(new Upgrade() { upgradeName = "Skill_One", currentLevel = 0 });
            GameData.sessionData.playerUpgrades.Add(new Upgrade() { upgradeName = "Skill_Two", currentLevel = 0 });
            GameData.sessionData.playerUpgrades.Add(new Upgrade() { upgradeName = "Skill_Three", currentLevel = 0 });
            GameData.sessionData.playerUpgrades.Add(new Upgrade() { upgradeName = "Skill_Four", currentLevel = 0 });
            GameData.sessionData.playerUpgrades.Add(new Upgrade() { upgradeName = "Skill_Five", currentLevel = 0 });
        }


        for (int i = 0; i < GameData.sessionData.playerUpgrades.Count; i++)
        {
            GameObject newUpgrade = Instantiate(upgradePrefab, content.position, content.rotation, content);
            newUpgrade.name = GameData.sessionData.playerUpgrades[i].upgradeName;

            newUpgrade.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = GameData.sessionData.playerUpgrades[i].upgradeName;
            newUpgrade.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = "(LVL:" + GameData.sessionData.playerUpgrades[i].currentLevel + ")";
            float upgradeCost()
            {
                if (GameData.sessionData.playerUpgrades[i].upgradeName == "Player")
                {
                    return 5 * (Mathf.Pow(1.062f, GameData.sessionData.playerUpgrades[i].currentLevel + 1) - Mathf.Pow(1.062f, GameData.sessionData.playerUpgrades[i].currentLevel)) / 0.062f;
                }
                else
                {
                    return 0f;
                }
            }

            

            newUpgrade.transform.Find("BuyBtn").Find("CostText").GetComponent<TextMeshProUGUI>().text = upgradeCost().ToString();

        }

      
    }
}
