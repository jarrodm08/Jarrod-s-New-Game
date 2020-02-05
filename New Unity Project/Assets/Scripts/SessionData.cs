using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SessionData : MonoBehaviour
{
    #region  Menu Settings
    public float musicVolume = 1f;
    #endregion  


    // Start is called before the first frame update
    void Start()
    {
        GameData data = SaveManager.LoadData();
        if (data != null)
        {
            Debug.Log("Loading Menu Settings");
            LoadMenuSettings(data); // Setup and load the menu settings
        }
        else
        {
            Debug.Log("data returned null, so we have no data to reference from");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadMenuSettings(GameData data)
    {
        musicVolume = data.musicVolume;
    }
}
