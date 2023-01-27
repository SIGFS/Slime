using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectAudio : MonoBehaviour
{
    public static LevelSelectAudio instance;

    public AudioSource audioFX;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Play_StartLevelSound()
    {
        audioFX.Play();
    }
}
