using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveManager
{
	public static void Save()
	{
	
		BinaryFormatter formatter = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
		formatter.Serialize(file, GameData.sessionData);
		file.Close();
		Debug.Log("Saved");
	}

	public static void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			GameData.sessionData = (GameData)formatter.Deserialize(file);
			file.Close();
		}
		else
		{
			GameData.sessionData = new GameData();
		}
	}
}


[System.Serializable]
public class GameData
{ //don't need ": Monobehaviour" because we are not attaching it to a game object

    public static GameData sessionData;

    public MenuSettings menuSettings;
	public PlayerData playerData;

	public UpgradeData playerUpgrade; //
	public UpgradeData[] heroUpgrades;
	

    public GameData()
    {
		menuSettings = new MenuSettings();
		playerData = new PlayerData();
		playerUpgrade = new UpgradeData();
		playerUpgrade.currentDamage = 1; // Default damage
		playerUpgrade.currentLevel = 1;
		heroUpgrades = new UpgradeData[2]; // set array size to number of upgrades in the game
		for (int i = 0; i < heroUpgrades.Length; i++)
		{
			heroUpgrades[i] = new UpgradeData();
		}
    }
}

