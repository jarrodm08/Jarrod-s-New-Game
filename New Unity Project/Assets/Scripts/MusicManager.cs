using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource music;

    void Start()
    {
        music = this.GetComponent<AudioSource>();
    }

    public void ChangeMusicVolume(float vol)
    {
        GameData.sessionData.menuSettings.musicVolume = vol;
        music.volume = vol;
    }
}
