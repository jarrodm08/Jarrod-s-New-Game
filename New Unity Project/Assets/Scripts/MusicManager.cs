using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioTrack;
    SessionData sessionData;

    void Awake()
    {
        DontDestroyOnLoad(this);
        return;
    }

    void Start()
    {
        sessionData = FindObjectOfType<SessionData>().GetComponent<SessionData>();
        audioTrack = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        audioTrack.volume = sessionData.musicVolume;
    }

    public void ChangeVolume(float vol)
    {
        sessionData.musicVolume = vol;
    }
}
