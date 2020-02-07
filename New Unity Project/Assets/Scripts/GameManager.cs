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
        else if(GameData.sessionData.playerData.monsterNum > 1 && spawnPos != null)
        {
            GameObject newMonster = Instantiate(monsterPrefabs[0], spawnPos.position, spawnPos.rotation, spawnPoints[1]);         
        }

    }

    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, spawnPoints[0].position, spawnPoints[0].rotation, spawnPoints[0]); // Spawn the first player
    }

    private Dictionary<string,TextMeshProUGUI> UIDic = new Dictionary<string, TextMeshProUGUI>();
    private void LoadUI()
    {
        Transform canvas = Camera.main.transform.Find("Canvas");

        spawnPoints = new Transform[2];
        spawnPoints[0] = canvas.Find("PlayerSpawn").Find("Spawn");
        spawnPoints[1] = canvas.Find("MonsterSpawn").Find("Spawn");
        

        monsterPrefabs = new GameObject[3];
        monsterPrefabs[0] = Resources.Load("Prefabs/MonsterA") as GameObject;
        //monsterPrefabs[1] = Resources.Load("Prefabs/MonsterB") as GameObject;
       // monsterPrefabs[2] = Resources.Load("Prefabs/MonsterC") as GameObject;

        playerPrefab = Resources.Load("Prefabs/Player") as GameObject;

        UIDic.Add("Gold",canvas.Find("Gold").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("Stage",canvas.Find("Stage").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("MonsterNum",canvas.Find("MonsterNum").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("PlayerDPS",canvas.Find("PlayerDPS").Find("PlayerDPSText").GetComponent<TextMeshProUGUI>());

        foreach (Button btn in canvas.Find("Tabs").GetComponentsInChildren<Button>())
        {
            GameObject panel = canvas.Find(btn.name).gameObject;
            panel.GetComponentInChildren<Button>().onClick.AddListener(() => TogglePanel(panel));
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




    

    //private void LoadUI()
    //{
    //    Transform canvas = Camera.main.transform.Find("Canvas");
    //    #region Load Panel Buttons
    //    panelBtns = new Button[4];
    //    panelBtns[0] = canvas.Find("ePanels").Find("Button1").GetComponent<Button>();
    //    panelBtns[0].onClick.AddListener(TogglePlayerUpgrades);
    //    panelBtns[1] = canvas.Find("ePanels").Find("Button2").GetComponent<Button>();
    //    panelBtns[1].onClick.AddListener(ToggleHeroUpgrades);
    //    panelBtns[2] = canvas.Find("ePanels").Find("Button3").GetComponent<Button>();
    //    panelBtns[2].onClick.AddListener(ToggleButton3Placeholder);
    //    panelBtns[3] = canvas.Find("ePanels").Find("Button4").GetComponent<Button>();
    //    panelBtns[3].onClick.AddListener(ToggleButton4Placeholder);
    //    #endregion

    //    displayGold = canvas.Find("Gold").Find("GoldText").GetComponent<TextMeshProUGUI>();
    //    displayStage = canvas.Find("StageIcon").Find("StageText").GetComponent<TextMeshProUGUI>();
    //    displayMonsterCount = canvas.Find("MonsterCount").Find("CountText").GetComponent<TextMeshProUGUI>();
    //    displayPlayerDPS = canvas.Find("PlayerDPS").Find("DPSText").GetComponent<TextMeshProUGUI>();
    //    displayHeroDPS = canvas.Find("HeroDPS").Find("DPSText").GetComponent<TextMeshProUGUI>();

    //    monsterAPrefab = Resources.Load("Prefabs/MonsterA") as GameObject;
    //    coinPrefab = Resources.Load("Prefabs/Coin") as GameObject;
    //}
    //private void SyncUI()
    //{
    //    displayGold.text = sessionData.playerGold + " GP";
    //    displayStage.text = sessionData.currentStage.ToString();
    //    displayMonsterCount.text = sessionData.stageCurrentMonster + "/" + sessionData.stageRequiredMonsters;
    //    displayPlayerDPS.text = sessionData.playerDPS.ToString();

    //}

    //private GameObject monsterAPrefab;
    //public Transform respawnPos;
    //public void spawnMonster()
    //{
    //    if (sessionData.stageCurrentMonster == 1 | respawnPos == null)
    //    { // Spawn first monster in the walking-spawn point further out
    //        GameObject monsterSpawn = Camera.main.transform.Find("Canvas").Find("MonsterSpawn").Find("Spawn").gameObject;
    //        GameObject monster = Instantiate(monsterAPrefab, monsterSpawn.transform.position, monsterSpawn.transform.rotation, monsterSpawn.transform); // Spawn the first player
    //        monster.GetComponent<Monster>().monsterMaxHP = Mathf.Round(17.5f * Mathf.Pow(1.39f, Mathf.Min(sessionData.currentStage, 115)) * Mathf.Pow(1.13f, Mathf.Max(sessionData.currentStage - 115, 0)));
    //        monster.GetComponent<Monster>().monsterCurrentHP = monster.GetComponent<Monster>().monsterMaxHP;
    //    }
    //    else
    //    {
    //        GameObject monster = Instantiate(monsterAPrefab, respawnPos.position, respawnPos.rotation, Camera.main.transform.Find("Canvas").Find("MonsterSpawn").Find("Spawn"));
    //        monster.GetComponent<Monster>().monsterMaxHP = Mathf.Round(17.5f * Mathf.Pow(1.39f, Mathf.Min(sessionData.currentStage, 115)) * Mathf.Pow(1.13f, Mathf.Max(sessionData.currentStage - 115, 0))); ;
    //        monster.GetComponent<Monster>().monsterCurrentHP = monster.GetComponent<Monster>().monsterMaxHP;
    //    }
    //}

    //private GameObject coinPrefab;

    //public void monsterDied(Transform spawnPos)
    //{
    //    if (sessionData.stageCurrentMonster <= 9)
    //    {
    //        sessionData.stageCurrentMonster += 1; //Upto the 10th monster spawn
    //        respawnPos = spawnPos;
    //        spawnMonster();
    //    }
    //    else
    //    {
    //        sessionData.currentStage += 1;
    //        sessionData.stageCurrentMonster = 1;
    //        spawnMonster();
    //    }

    //    GameObject newCoin = Instantiate(coinPrefab, spawnPos.position, spawnPos.rotation, Camera.main.transform.Find("Canvas"));
    //    newCoin.GetComponent<Coin>().coinValue = Mathf.CeilToInt(Mathf.Round(17.5f * Mathf.Pow(1.39f, Mathf.Min(sessionData.currentStage, 115)) * Mathf.Pow(1.13f, Mathf.Max(sessionData.currentStage - 115, 0))) * (0.008f + 0.002f * Mathf.Min(sessionData.currentStage, 150)));
    //    newCoin.GetComponent<Coin>().target = displayGold.transform.parent.Find("MoneyIcon");
    //}


    //#region Panel Buttons
    //private void TogglePlayerUpgrades()
    //{
    //    Transform panel = Camera.main.transform.Find("Canvas").Find("PlayerUpgrades") ;
    //    if (panel.gameObject.activeSelf == false)
    //    {
    //        panel.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        panel.gameObject.SetActive(false);
    //    }
    //}
    //private void ToggleHeroUpgrades()
    //{
    //    Debug.Log("Toggle Hero Upgrades");
    //}
    //private void ToggleButton3Placeholder()
    //{
    //    Debug.Log("Toggle btn3placeholder Upgrades");
    //}
    //private void ToggleButton4Placeholder()
    //{
    //    Debug.Log("Toggle btn4placeholder Upgrades");
    //}
    //#endregion


    //void OnApplicationQuit()
    //{
    //    //SaveManager.SaveData(this.GetComponent<SessionData>());
    //}
}

