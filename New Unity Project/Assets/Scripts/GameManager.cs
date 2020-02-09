using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(this);
        UpgradeUtils upUtils = new UpgradeUtils();
        RoundingUtils roUtils = new RoundingUtils();
    }

    void Start()
    {

        SaveManager.Load();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        LoadUI();
        syncing = true; // Start syncing UI values
        SpawnPlayer();
        SpawnMonster();
    }

    private bool syncing = false;
    void Update()
    {
        if (syncing == true)
        {
            SyncUI();
        }
    }

    private Transform[] spawnPoints;
    private GameObject[] monsterPrefabs;
    private GameObject playerPrefab;

    public void SpawnMonster(Transform spawnPos = null)
    {
        if (GameData.sessionData.playerData.monsterNum == 1 | spawnPos == null)
        {
            GameObject newMonster = Instantiate(monsterPrefabs[0], spawnPoints[1].position, spawnPoints[1].rotation, spawnPoints[1]); // Spawn the first player
        }
        else if (GameData.sessionData.playerData.monsterNum > 1 && spawnPos != null)
        {
            GameObject newMonster = Instantiate(monsterPrefabs[0], spawnPos.position, spawnPos.rotation, spawnPoints[1]);
        }

    }

    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, spawnPoints[0].position, spawnPoints[0].rotation, spawnPoints[0]); // Spawn the first player
    }

    private Dictionary<string, TextMeshProUGUI> UIDic = new Dictionary<string, TextMeshProUGUI>();
    private void LoadUI()
    {
        Transform canvas = Camera.main.transform.Find("Canvas");

        #region Player/Monster
        spawnPoints = new Transform[2];
        spawnPoints[0] = canvas.Find("PlayerSpawn").Find("Spawn");
        spawnPoints[1] = canvas.Find("MonsterSpawn").Find("Spawn");


        monsterPrefabs = new GameObject[3];
        monsterPrefabs[0] = Resources.Load("Prefabs/MonsterA") as GameObject;
        //monsterPrefabs[1] = Resources.Load("Prefabs/MonsterB") as GameObject;
        // monsterPrefabs[2] = Resources.Load("Prefabs/MonsterC") as GameObject;

        playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
        #endregion

        UIDic.Add("Gold", canvas.Find("Gold").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("Stage", canvas.Find("Stage").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("MonsterNum", canvas.Find("MonsterNum").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("PlayerDPS", canvas.Find("PlayerDPS").Find("PlayerDPSText").GetComponent<TextMeshProUGUI>());
        UIDic.Add("HeroDPS",canvas.Find("HeroDPS").Find("DPSText").GetComponent<TextMeshProUGUI>());
        foreach (Button btn in canvas.Find("Tabs").GetComponentsInChildren<Button>())
        {
            GameObject panel = canvas.Find(btn.name).gameObject;
            panel.GetComponentInChildren<Button>().onClick.AddListener(() => TogglePanel(panel));
            
            if (panel.name == "Panel2")
            {
                heroUpgradePanel = panel;
                panel.transform.Find("Scroll View").GetComponent<ScrollRect>().verticalNormalizedPosition = 0; // Scroll to the very bottom
            }
            btn.onClick.AddListener(() => TogglePanel(panel));
        }

        void TogglePanel(GameObject panel)
        {
            if (panel.activeSelf == false)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    private GameObject heroUpgradePanel;
    private void SyncUI()
    {
        UIDic["Gold"].text = RoundingUtils.GetShorthand(GameData.sessionData.playerData.gold);
        UIDic["Stage"].text = GameData.sessionData.playerData.stage.ToString();
        UIDic["MonsterNum"].text = GameData.sessionData.playerData.monsterNum.ToString() + "/10";
        UIDic["PlayerDPS"].text = GameData.sessionData.playerData.tapDamage.ToString();
        UIDic["HeroDPS"].text = UpgradeUtils.GetTotalHeroDPS().ToString();

        UpgradeUtils.UnlockNextHero(heroUpgradePanel);
    }



    void OnApplicationQuit()
    {      
        
        SaveManager.Save();
    }

    
    

}


public class UpgradeUtils
{
    public static int heroCostMultiplier = -9;
    public static int GetImprovementBonus(int level,bool checkNextLevel = false)
    {
        int multiplier = 1;
        foreach (KeyValuePair<int, int> p in improvementBonusDic)
        {
            if (checkNextLevel == false && level >= p.Key)
            {
                multiplier += (p.Value - 1);
            }
            else if (checkNextLevel == true && (level + 1) >= p.Key)
            {
                multiplier += (p.Value - 1);
            }
        }
        return multiplier;
    }
    public static float[] CalculateUpgrade(string name, int level, float baseCost, int heroUnlockOrder)
    {
        float[] results = new float[2];
        if (name == "Player")
        {
            //Cost
            results[0] = Mathf.Round(5 * (Mathf.Pow(1.062f, level + 1) - Mathf.Pow(1.062f, level)) / 0.062f);
            //Damage
            results[1] = ((level + 1) * GetImprovementBonus(level,true)) - ((level) * GetImprovementBonus(level)); // difference between current damage and what it would be +1 level
        }
        else
        {
            //Cost
            results[0] = Mathf.Round(baseCost * (Mathf.Pow(1.082f, level)) * (Mathf.Pow(1.082f, 1) - 1) / 0.82f * (1 - heroCostMultiplier));
            //Damage
            float currentDamage = ((baseCost / 10 * (1 - 23 / 1000 * Mathf.Pow(Mathf.Min(heroUnlockOrder, 34), Mathf.Min(heroUnlockOrder, 34))) * (level) * GetImprovementBonus(level)));
            results[1] = ((baseCost / 10 * (1 - 23 / 1000 * Mathf.Pow(Mathf.Min(heroUnlockOrder, 34), Mathf.Min(heroUnlockOrder, 34))) * (level + 1) * GetImprovementBonus(level, true)) - currentDamage);
        }
        return results;
    }
    public static float GetTotalHeroDPS()
    {
        float dps = 0;
        for (int i = 0; i < GameData.sessionData.heroUpgrades.Length; i++)
        {
            dps += GameData.sessionData.heroUpgrades[i].currentDamage;
        }
        return dps;
    }
    public static void UnlockNextHero(GameObject panel)
    {
        Upgrade[] upgrades = panel.GetComponentsInChildren<Upgrade>(true);
        for (int i = upgrades.Length*2; i > upgrades.Length; i--)
        {
            Upgrade hero = upgrades[i - upgrades.Length - 1];
            if (hero.gameObject.activeSelf == false)
            {
                if (GameData.sessionData.playerData.gold >= CalculateUpgrade(hero.upgradeName, hero.currentLevel - 1, hero.baseCost, hero.heroUnlockOrder)[0] * 0.10f || GameData.sessionData.heroUpgrades[hero.heroUnlockOrder - 1].unlocked == true)
                {
                    hero.gameObject.SetActive(true);
                    GameData.sessionData.heroUpgrades[hero.heroUnlockOrder - 1].unlocked = true;
                    break;
                } 
            }
        }
    }

    public UpgradeUtils()
    {
        InitImprovementBonus();
    }

    private static Dictionary<int, int> improvementBonusDic;
    private void InitImprovementBonus()
    { 
        improvementBonusDic = new Dictionary<int, int>();
        improvementBonusDic.Add(10, 2); // Level 10 = 200% Damage (100% increase)
    }
}
public class RoundingUtils 
{
    
    public static string GetShorthand(float number)
    {
        foreach (KeyValuePair<float,string> p in shorthandDic)
        {
            if (number >= Mathf.Pow(10,15) && number >= p.Key && number <= p.Key *10)
            {//Scientific Notation
                return Math.Round(number / p.Key, 2).ToString() + p.Value;
            }
            else if (number <= Mathf.Pow(10,15) && number >= p.Key && number <= p.Key*1000)
            {// regular notation
                return Math.Round(number / p.Key, 2).ToString() + p.Value;
            }
        }
        return number.ToString();
    }

    public RoundingUtils()
    {
        InitShorthandDic();
    }
    public static Dictionary<float, string> shorthandDic;
    private void InitShorthandDic()
    {
        shorthandDic = new Dictionary<float, string>();
        shorthandDic.Add(Mathf.Pow(10, 3), " K");
        shorthandDic.Add(Mathf.Pow(10, 6), " M");
        shorthandDic.Add(Mathf.Pow(10, 9), " B");
        shorthandDic.Add(Mathf.Pow(10, 12), " T");
        shorthandDic.Add(Mathf.Pow(10, 15), " e+15");
        shorthandDic.Add(Mathf.Pow(10, 16), " e+16");
        shorthandDic.Add(Mathf.Pow(10, 17), " e+17");
        shorthandDic.Add(Mathf.Pow(10, 18), " e+18");
        shorthandDic.Add(Mathf.Pow(10,21), " e+21");
        shorthandDic.Add(Mathf.Pow(10, 24), " e+24");
    }
}
