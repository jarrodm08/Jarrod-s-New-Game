using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SessionData : MonoBehaviour
{
    #region  Menu Settings
    public float musicVolume = 1f;
    #endregion
    #region Player Info
    public float playerGold = 0f;
    public float playerDPS = 23f;
    #endregion
    #region StageInfo
    public int currentStage = 1;
    public float stageRequiredMonsters = 10f;
    public float stageCurrentMonster = 1f;
    #endregion

    void Awake()
    {
        DontDestroyOnLoad(this);
        return;
    }

    void Start()
    {
        GameData data = SaveManager.LoadData();
        if (data != null)
        {
            Debug.Log("Loading Menu Settings");
            LoadMenuSettings(data); // Setup and load the menu settings
            LoadPlayerData(data);
            LoadStageData(data);
        }
        else
        {
            Debug.Log("data returned null, so we have no data to reference from");
        }
    }

    void Update()
    {
        
    }

    private void LoadMenuSettings(GameData data)
    {
        musicVolume = data.musicVolume;
    }
    private void LoadPlayerData(GameData data)
    {
        playerGold = data.playerGold;
        playerDPS = data.playerDPS;
    }
    private void LoadStageData(GameData data)
    {
        currentStage = data.currentStage;
        stageCurrentMonster = data.stageCurrentMonster;
        stageRequiredMonsters = data.stageRequiredMonsters;
    }
}
