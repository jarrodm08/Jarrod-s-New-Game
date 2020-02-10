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

    private Dictionary<string, TextMeshProUGUI> UIDic = new Dictionary<string, TextMeshProUGUI>(); // Main UI Text's
    private Dictionary<string, GameObject> upgradePanelsDIc = new Dictionary<string, GameObject>(); // Panels Of The Tab/Buttons
    private Transform[] spawnPoints;
    private GameObject[] monsterPrefabs;
    private GameObject playerPrefab;

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

        #region Main UI
        UIDic.Add("Gold", canvas.Find("Gold").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("Stage", canvas.Find("Stage").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("MonsterNum", canvas.Find("MonsterNum").GetComponentInChildren<TextMeshProUGUI>());
        UIDic.Add("PlayerDPS", canvas.Find("PlayerDPS").Find("PlayerDPSText").GetComponent<TextMeshProUGUI>());
        UIDic.Add("HeroDPS",canvas.Find("HeroDPS").Find("DPSText").GetComponent<TextMeshProUGUI>());
        #endregion

        #region Upgrade Tabs
        foreach (Button btn in canvas.Find("Tabs").GetComponentsInChildren<Button>())
        {           
            GameObject panel = canvas.Find(btn.name).gameObject;
            panel.GetComponentInChildren<Button>().onClick.AddListener(() => TogglePanel(panel)); // Main UI Button
            btn.onClick.AddListener(() => TogglePanel(panel)); // Exit Button
            upgradePanelsDIc.Add(btn.name, canvas.Find(btn.name).gameObject);
        }

        upgradePanelsDIc["Panel2"].transform.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 0; // Scroll to the very bottom
      
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
        #endregion

        if (GameData.sessionData.heroUpgrades[0].unlocked == true)
        {
            Debug.Log("We have fluffers unlocked, so change sprite");
        }
    }

    private bool changeSprite = false;
    private void SyncUI()
    {
        UIDic["Gold"].text = RoundingUtils.GetShorthand(GameData.sessionData.playerData.gold);
        UIDic["Stage"].text = GameData.sessionData.playerData.stage.ToString();
        UIDic["MonsterNum"].text = GameData.sessionData.playerData.monsterNum.ToString() + "/10";
        UIDic["PlayerDPS"].text = RoundingUtils.GetShorthand(GameData.sessionData.playerData.tapDamage);
        UIDic["HeroDPS"].text = RoundingUtils.GetShorthand(UpgradeUtils.GetTotalHeroDPS());

        UpgradeUtils.UnlockNextHero(upgradePanelsDIc["Panel2"]);
        ///// ****** TRIGGER DIALOGUE TO BUY FIRST UPGRADE AFTER 30 GOLD
        if (GameData.sessionData.heroUpgrades[0].unlocked == true && changeSprite == false)
        {
            changeSprite = true;
            Camera.main.transform.Find("Canvas").Find("Tabs").Find("Panel2").GetComponent<Image>().color = new Color32(50,92,233,255);
            Camera.main.transform.Find("Canvas").Find("Tabs").Find("Panel2").Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/HeroUpgrades") as Sprite;
            Camera.main.transform.Find("Canvas").Find("Tabs").Find("Panel2").Find("Icon").GetComponent<Image>().color = new Color32(255,255,255,200);
        }
    }


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

    void OnApplicationQuit()
    {             
        SaveManager.Save();
    }
}


