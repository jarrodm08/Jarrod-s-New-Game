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
        UpgradeUtils util = new UpgradeUtils();
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

        foreach (Button btn in canvas.Find("Tabs").GetComponentsInChildren<Button>())
        {
            GameObject panel = canvas.Find(btn.name).gameObject;
            panel.GetComponentInChildren<Button>().onClick.AddListener(() => TogglePanel(panel));
            btn.onClick.AddListener(() => TogglePanel(panel));

            Upgrade[] upgrades = panel.GetComponentsInChildren<Upgrade>(); // Loads all upgrades in the content
            for (int i = 0; i < upgrades.Length; i++)
            {

            }

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

    private void SyncUI()
    {
        UIDic["Gold"].text = GameData.sessionData.playerData.gold.ToString();
        UIDic["Stage"].text = GameData.sessionData.playerData.stage.ToString();
        UIDic["MonsterNum"].text = GameData.sessionData.playerData.monsterNum.ToString();
        UIDic["PlayerDPS"].text = GameData.sessionData.playerData.tapDamage.ToString();
    }



    void OnApplicationQuit()
    {      
        
        SaveManager.Save();
    }

    
    

}


public class UpgradeUtils
{
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
    
    public static int heroCostMultiplier = -9;

    public static float GetUpgradeCost(string name,int level,float baseCost)
    {
       
        if (name == "Player")
        {
            return Mathf.Round(5 * (Mathf.Pow(1.062f, level + 1) - Mathf.Pow(1.062f, level)) / 0.062f);
        }
        else
        {
            return Mathf.Round(baseCost * (Mathf.Pow(1.082f, level + 1)) * (Mathf.Pow(1.082f, 1) - 1) / 0.82f * (1 - heroCostMultiplier));
        }
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
