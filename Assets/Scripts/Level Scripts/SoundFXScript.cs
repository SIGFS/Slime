using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXScript : MonoBehaviour
{
    public static SoundFXScript instance;
    public AudioSource soundFX;
    public AudioClip Enemy, SlimeHit, Jump, Walk, Water;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void hitSound()
    {
        soundFX.clip = SlimeHit;
        soundFX.Play();
    }

    public void enemySound()
    {
        soundFX.clip = Enemy;
        soundFX.Play();
    }

    public void jumpSound()
    {
        soundFX.clip = Jump;
        soundFX.Play();
    }

    public void walkSound()
    {
        soundFX.clip = Walk;
        soundFX.Play();
    }

    public void waterSound()
    {
        soundFX.clip = Water;
        soundFX.Play();
    }

}
