using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuSettings
{
    public float musicVolume;
    public float effectsVolume;

    public MenuSettings()
    {
        //Set Defaults Here
        this.musicVolume = 1f;
        this.effectsVolume = 1f;
    }
}
