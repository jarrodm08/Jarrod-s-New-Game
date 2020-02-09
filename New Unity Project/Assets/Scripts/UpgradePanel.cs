using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpgradePanel : MonoBehaviour
{
    private Transform content;
    private GameObject upgradePrefab;

    void Start()
    {
        content = this.transform.Find("Scroll View").Find("Viewport").Find("Content");
        upgradePrefab = Resources.Load("Prefabs/Upgrade") as GameObject;

        //LoadUpgrades();
    }

    private bool startSync = false;

    void Update()
    {
        if (startSync == true)
        {
            //SyncUI();
        }    
    }

    private Dictionary<GameObject, float> upgradeCostDic = new Dictionary<GameObject, float>();
    //public void LoadUpgrades()
    //{
        //if (gamedata.sessiondata.playerupgrades.count == 0)
        //{
        //    debug.log("no upgrades saved, create new upgrades now");
        //    gamedata.sessiondata.playerupgrades.add(new upgrade() {upgradename = "player",currentlevel = 1});
        //    gamedata.sessiondata.playerupgrades.add(new upgrade() { upgradename = "skill_one", currentlevel = 0, basecost = 10 });
        //    gamedata.sessiondata.playerupgrades.add(new upgrade() { upgradename = "skill_two", currentlevel = 0, basecost = 100 });
        //    gamedata.sessiondata.playerupgrades.add(new upgrade() { upgradename = "skill_three", currentlevel = 0, basecost = 1000});
        //    gamedata.sessiondata.playerupgrades.add(new upgrade() { upgradename = "skill_four", currentlevel = 0, basecost = 10000 });
        //    gamedata.sessiondata.playerupgrades.add(new upgrade() { upgradename = "skill_five", currentlevel = 0, basecost = 100000 });
        //}

        //float upgradecost(int i)
        //{
        //    if (gamedata.sessiondata.playerupgrades[i].upgradename == "player")
        //    {
        //     return 5 * (mathf.pow(1.062f, gamedata.sessiondata.playerupgrades[i].currentlevel + 1) - mathf.pow(1.062f, gamedata.sessiondata.playerupgrades[i].currentlevel)) / 0.062f;
        //    }
        //    else
        //    {
        //    return gamedata.sessiondata.playerupgrades[i].basecost * mathf.pow(1.082f, gamedata.sessiondata.playerupgrades[i].currentlevel)*(1.082f-1)/0.82f*(1-0);
        //    }
            
        //}

        
    //    for (int i = 0; i < GameData.sessionData.playerUpgrades.Count; i++)
    //    {
    //        GameObject newUpgrade = Instantiate(upgradePrefab, content.position, content.rotation, content);
    //        newUpgrade.name = GameData.sessionData.playerUpgrades[i].upgradeName;

    //        newUpgrade.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = GameData.sessionData.playerUpgrades[i].upgradeName;
    //        newUpgrade.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = "(LVL:" + GameData.sessionData.playerUpgrades[i].currentLevel + ")";
    //        newUpgrade.transform.Find("BuyBtn").Find("CostText").GetComponent<TextMeshProUGUI>().text = upgradeCost(i).ToString();
    //        newUpgrade.transform.Find("BuyBtn").GetComponent<Button>().onClick.AddListener(() => BuyUpgrade(i-1,newUpgrade));
    //        upgradeCostDic.Add(newUpgrade.transform.Find("BuyBtn").gameObject,upgradeCost(i));
            

    //    }


    //    void BuyUpgrade(int index,GameObject upgrade)
    //    {
    //        if (GameData.sessionData.playerData.gold - upgradeCost(index+1) >= 0)
    //        {
    //            GameData.sessionData.playerUpgrades[index].currentLevel += 1;
    //            upgrade.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = "(LVL:" + GameData.sessionData.playerUpgrades[index].currentLevel + ")";
    //            Debug.Log(upgradeCost(index+1));

    //            Debug.Log("You can afford this");
    //        }
    //        else
    //        {
    //            Debug.Log("You cant afford this. You have: " + GameData.sessionData.playerData.gold + " . And the uograde costs: " +upgradeCost(index+1));
    //        }
    //    }

    //    startSync = true;
    //}
    //Color32 canBuy = new Color32(215,147,0,255);
    //Color32 cantBuy = new Color32(113,113,113,255);
    //private void SyncUI()
    //{
        
    //    foreach (KeyValuePair<GameObject,float> p in upgradeCostDic)
    //    {
    //        if (GameData.sessionData.playerData.gold - p.Value >= 0)
    //        {
    //            p.Key.GetComponent<Image>().color = canBuy;
    //        }
    //        else
    //        {
    //            p.Key.GetComponent<Image>().color = cantBuy;
    //        }
    //    }
    //}
}
