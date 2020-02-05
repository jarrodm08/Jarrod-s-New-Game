using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{

    public static void SaveData(SessionData sessionData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.anyExtension";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(sessionData); // Use game data to parse confusing/complicated data we have stored in our game to a file-friendly format
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/player.anyExtension";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("No Save File Exists");
            return null;
        }
    }
}

[System.Serializable]
public class GameData
{
    #region Menu Settings
    public float musicVolume;
    #endregion
    #region Player Settings
    public float playerGold;
    public float playerDPS;
    #endregion
    #region StageInfo
    public int currentStage;
    public float stageRequiredMonsters;
    public float stageCurrentMonster;
    #endregion

    public GameData(SessionData saveData)
    {
        //Here we need to prase session data in a save-able format, so convert any vector3's to arrays, ect so that we can save this class
        SaveMenuSettings(saveData); // Save Menu Settings
        SavePlayerData(saveData); // Save Player Data
        SaveStageData(saveData); // Save Stage data
    }

    private void SaveMenuSettings(SessionData data)
    {
        musicVolume = data.musicVolume;
    }
    private void SavePlayerData(SessionData data)
    {
        playerGold = data.playerGold;
        playerDPS = data.playerDPS;
    }
    private void SaveStageData(SessionData data)
    {
        currentStage = data.currentStage;
        stageRequiredMonsters = data.stageRequiredMonsters;
        stageCurrentMonster = data.stageCurrentMonster;
    }
}
