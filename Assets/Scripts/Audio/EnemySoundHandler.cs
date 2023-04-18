using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] Batflaps;
    [SerializeField] private AudioClip[] BoneRattles;
    [SerializeField] private AudioClip[] SlimeMovement;

    public bool isBat = false;
    public bool isBones = false;
    public bool isSlime = false;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(playEnemySound());
    }

    private void playsound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    public void playBatFlap()
    {
        playsound(Batflaps[Random.Range(0, Batflaps.Length)]);
    }
    public void playBoneRattle()
    {
        playsound(BoneRattles[Random.Range(0, BoneRattles.Length)]);
    }

    public void playMovement()
    {
        playsound(SlimeMovement[Random.Range(0, SlimeMovement.Length)]);
    }

    IEnumerator playEnemySound()
    {
        if (isBat)
        {
            playBatFlap();
            yield return new WaitForSeconds(Random.Range(.75f, 1.0f));
        }
        else if (isBones)
        {
            playBoneRattle();
            yield return new WaitForSeconds(Random.Range(1.5f, 2.0f));
        }
        else if (isSlime)
        {
            playMovement();
            yield return new WaitForSeconds(Random.Range(1.5f, 2.0f));
        }
        //yield return new WaitForSeconds(Random.Range(1.5f, 2.0f));
        StartCoroutine(playEnemySound());
    }
}
