using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        //this.GetComponent<AudioSource>().volume = FindObjectOfType<SessionData>().musicVolume;
    }

    public void ChangeVolume(float vol)
    {
        //sessionData.musicVolume = vol;
    }
}
