using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private SessionData sessionData;


    void Start()
    {
        sessionData = FindObjectOfType<SessionData>();
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }


    void Update()
    {
        if (gameLoaded == true)
        {
            SyncUI();
        }
    }


    private bool gameLoaded = false;

    private GameObject canvas;
    private TextMeshProUGUI displayGold;
    private TextMeshProUGUI displayMonsterCount;
    private TextMeshProUGUI displayStage;
    private TextMeshProUGUI displayPlayerDPS;
    private TextMeshProUGUI displayHeroDPS;

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading; // Disable the listner for scene changes
        LoadUI();
        gameLoaded = true;


        GameObject playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
        GameObject playerSpawn = Camera.main.transform.Find("Canvas").Find("PlayerSpawn").Find("Spawn").gameObject;

        GameObject player = Instantiate(playerPrefab,playerSpawn.transform.position,playerSpawn.transform.rotation,playerSpawn.transform); // Spawn the first player
       
        spawnMonster();
    }

    private void LoadUI()
    {
        canvas = Camera.main.transform.Find("Canvas").gameObject;
        displayGold = canvas.transform.Find("Gold").Find("GoldText").GetComponent<TextMeshProUGUI>();
        displayStage = canvas.transform.Find("StageIcon").Find("StageText").GetComponent<TextMeshProUGUI>();
        displayMonsterCount = canvas.transform.Find("MonsterCount").Find("CountText").GetComponent<TextMeshProUGUI>();
        displayPlayerDPS = canvas.transform.Find("PlayerDPS").Find("DPSText").GetComponent<TextMeshProUGUI>();
        displayHeroDPS = canvas.transform.Find("HeroDPS").Find("DPSText").GetComponent<TextMeshProUGUI>();

        monsterAPrefab = Resources.Load("Prefabs/MonsterA") as GameObject;
    }
    private void SyncUI()
    {
        displayGold.text = sessionData.playerGold + " GP";
        displayStage.text = sessionData.currentStage.ToString();
        displayMonsterCount.text = sessionData.stageCurrentMonster + "/" + sessionData.stageRequiredMonsters;
        displayPlayerDPS.text = sessionData.playerDPS.ToString();

    }

    private GameObject monsterAPrefab;
    public Transform respawnPos;
    public void spawnMonster()
    {
        if (sessionData.stageCurrentMonster == 1)
        { // Spawn first monster in the walking-spawn point further out
            GameObject monsterSpawn = Camera.main.transform.Find("Canvas").Find("MonsterSpawn").Find("Spawn").gameObject;
            GameObject monster = Instantiate(monsterAPrefab, monsterSpawn.transform.position, monsterSpawn.transform.rotation, monsterSpawn.transform); // Spawn the first player
            monster.GetComponent<Monster>().monsterMaxHP = Mathf.Round(17.5f * Mathf.Pow(1.39f, Mathf.Min(sessionData.currentStage, 115)) * Mathf.Pow(1.13f, Mathf.Max(sessionData.currentStage - 115, 0)));
            monster.GetComponent<Monster>().monsterCurrentHP = monster.GetComponent<Monster>().monsterMaxHP;
        }
        else
        {
            GameObject monster = Instantiate(monsterAPrefab, respawnPos.position, respawnPos.rotation, Camera.main.transform.Find("Canvas").Find("MonsterSpawn").Find("Spawn"));
            monster.GetComponent<Monster>().monsterMaxHP = Mathf.Round(17.5f * Mathf.Pow(1.39f, Mathf.Min(sessionData.currentStage, 115)) * Mathf.Pow(1.13f, Mathf.Max(sessionData.currentStage - 115, 0))); ;
            monster.GetComponent<Monster>().monsterCurrentHP = monster.GetComponent<Monster>().monsterMaxHP;
        }
    }

    public void monsterDied(Transform spawnPos)
    {
        if (sessionData.stageCurrentMonster <= 9)
        {
            sessionData.stageCurrentMonster += 1; //Upto the 10th monster spawn
            respawnPos = spawnPos;
            spawnMonster();
        }
        else
        {
            sessionData.currentStage += 1;
            sessionData.stageCurrentMonster = 1;
            spawnMonster();
        }
    }
}

