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

    private Dictionary<string, Transform> UIDic = new Dictionary<string, Transform>();
    private GameObject[] characterPrefabs; // Holds Players and Monsters
    private List<string> uiNames = new List<string>()
        {
            "Canvas", // [0]

            "MainUI", // [1]
            "UpgradePanelsUI", //[2]

            "ButtonsContainer", // [3]
            "GoldDisplay", // [4]
            "StageDisplay",// [5]
            "MonsterCountDisplay",// [6]
            "PlayerDPSDisplay",// [7]
            "HeroDPSDisplay", // [8]
            
            "PlayerUpgrades", // [9]
            "HeroUpgrades", // [10]
            "Panel3", // [11]
            "Panel4", // [12]

            "PlayerSpawn", // [13]
            "MonsterSpawn" //[14]
            
        };
    private void LoadUI()
    {
        UIDic.Add(uiNames[0], Camera.main.transform.Find(uiNames[0])); // Canvas

        #region Main UI 
        for (int i = 1; i < uiNames.Count - 2; i++)
        {
            Transform pTransform = null;
            Transform cTransform = null;

            if (i < 3)
            {
                pTransform = UIDic[uiNames[0]]; // Sets MainUI && UpgradePanelsUI
            }
            else if (i >= 3 && i < 9)
            {
                pTransform = UIDic[uiNames[1]]; // Sets MainUI as PARENT
            }
            else if ( i >= 9 && i < 13)
            {
                pTransform = UIDic[uiNames[2]]; // Sets UpgradePanelsUI as PARENT
            }
            cTransform = pTransform.Find(uiNames[i]); // Gets the child of the parent
            UIDic.Add(uiNames[i], cTransform);
        }
        #endregion
        
        #region Player/Monster
        //Loads Player And Monster Prefabs
        characterPrefabs = Resources.LoadAll<GameObject>("Prefabs/Characters");
        UIDic.Add(uiNames[13], UIDic[uiNames[0]].Find(uiNames[13]).Find("Spawn")); // Player Spawn
        UIDic.Add(uiNames[14], UIDic[uiNames[0]].Find(uiNames[14]).Find("Spawn")); // Monster Spawn
        #endregion

        foreach (Button btn in UIDic[uiNames[3]].GetComponentsInChildren<Button>())
        {
            if (btn.name != uiNames[10])
            { // For uiNames[9] (HeroUpgrades) Keep locked until we trigger the unlock event
                btn.onClick.AddListener(() => TogglePanel(UIDic[btn.name].gameObject));
            }
            UIDic[btn.name].GetComponentInChildren<Button>().onClick.AddListener(() => TogglePanel(UIDic[btn.name].gameObject)); // Set exit button listener
        }
    }

    private void TogglePanel(GameObject panel)
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

    private bool changeSprite = false;
    private void SyncUI()
    {
        UIDic[uiNames[4]].GetComponentInChildren<TextMeshProUGUI>().text = RoundingUtils.GetShorthand(GameData.sessionData.playerData.gold);
        UIDic[uiNames[5]].GetComponentInChildren<TextMeshProUGUI>().text = GameData.sessionData.playerData.stage.ToString();
        UIDic[uiNames[6]].GetComponentInChildren<TextMeshProUGUI>().text = GameData.sessionData.playerData.monsterNum.ToString() + "/10";
        UIDic[uiNames[7]].GetComponentInChildren<TextMeshProUGUI>().text = RoundingUtils.GetShorthand(GameData.sessionData.playerUpgrade.currentDamage);
        UIDic[uiNames[8]].GetComponentInChildren<TextMeshProUGUI>().text = RoundingUtils.GetShorthand(UpgradeUtils.GetTotalHeroDPS());

        UpgradeUtils.UnlockNextHero(UIDic[uiNames[10]].gameObject);
        if (GameData.sessionData.heroUpgrades[0].unlocked == true && changeSprite == false)
        {
            Transform btn = UIDic[uiNames[3]].Find(uiNames[10]);
            btn.GetComponentInChildren<Button>().onClick.AddListener(() => TogglePanel(UIDic[uiNames[10]].gameObject));
            btn.GetComponentInChildren<Image>().color = new Color32(50, 92, 233, 255); // Brighten the button
            btn.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/HeroUpgrades") as Sprite; // Set button sprite
            btn.Find("Icon").GetComponent<Image>().color = new Color32(255, 255, 255, 200); // Change Icon Brightness
            changeSprite = true;
        }
    }


    public void SpawnMonster(Transform spawnPos = null)
    {
        if (GameData.sessionData.playerData.monsterNum == 1 | spawnPos == null)
        {
            Instantiate(characterPrefabs[0], UIDic[uiNames[14]].position, UIDic[uiNames[14]].rotation, UIDic[uiNames[14]]); // Spawn First Monster
        }
        else if (GameData.sessionData.playerData.monsterNum > 1 && spawnPos != null)
        {
            Instantiate(characterPrefabs[0], spawnPos.position, spawnPos.rotation, UIDic[uiNames[14]]); // Respawn
        }
    }

    private void SpawnPlayer()
    {
        Instantiate(characterPrefabs[characterPrefabs.Length - 1],UIDic[uiNames[13]].position, UIDic[uiNames[13]].rotation, UIDic[uiNames[13]]);     
    }

    void OnApplicationQuit()
    {             
        SaveManager.Save();
    }
}


