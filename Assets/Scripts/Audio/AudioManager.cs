using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _musicSource, _effectsSource;
    [SerializeField] private AudioClip[] Batflaps;
    [SerializeField] private AudioClip[] BoneRattles;
    [SerializeField] private AudioClip[] SlimeLand;
    [SerializeField] private AudioClip[] SlimeMovement;
    [SerializeField] private AudioClip[] SlimeBounce;
    [SerializeField] private AudioClip[] SlimeGrow;
    [SerializeField] private AudioClip SlimeShot;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void playsound(AudioClip clip)
    {
        _effectsSource.clip = clip;
        _effectsSource.PlayOneShot(clip);
    }

    public void playBatFlap()
    {
        playsound(Batflaps[Random.Range(0, Batflaps.Length)]);
    }

    public void playBoneRattle()
    {
        playsound(BoneRattles[Random.Range(0, BoneRattles.Length)]);
    }

    public void playSlimeJumpLand()
    {
        playsound(SlimeLand[Random.Range(0, SlimeLand.Length)]);
    }

    public void playMovement()
    {
        playsound(SlimeMovement[Random.Range(0, SlimeMovement.Length)]);
    }

    public void playSlimeBounce()
    {
        playsound(SlimeBounce[Random.Range(0, SlimeBounce.Length)]);
    }

    public void playGrow()
    {
        playsound(SlimeGrow[Random.Range(0, SlimeGrow.Length)]);
    }

    public void playSlimeShot()
    {
        playsound(SlimeShot);
    }
}
