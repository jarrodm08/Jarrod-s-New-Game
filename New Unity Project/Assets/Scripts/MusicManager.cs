using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private float musicVolume = 1f;
    private AudioSource audioTrack;


    void Awake()
    {
        DontDestroyOnLoad(this);
        return;
    }

    void Start()
    {
        audioTrack = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        audioTrack.volume = musicVolume;
    }

    public void ChangeVolume(float vol)
    {
        musicVolume = vol;
    }
}
