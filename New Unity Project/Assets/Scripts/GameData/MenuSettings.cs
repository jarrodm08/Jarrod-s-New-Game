using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuSettings
{
    public float musicVolume;

    public MenuSettings()
    {
        //Set Defaults Here
        this.musicVolume = 1f;
    }
}
