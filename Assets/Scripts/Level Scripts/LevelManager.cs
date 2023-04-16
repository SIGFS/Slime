using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private GameObject player;
    void Start()
    {
        if(checkpoints[PlayerPrefs.GetInt("CurrentCheckpoint")] != null)
        {
            player.transform.position = checkpoints[PlayerPrefs.GetInt("CurrentCheckpoint")].transform.position;
        }
        
    }

    public void ResetCheckpoints()
    {
        PlayerPrefs.SetInt("CurrentCheckpoint", 0);
    }
}
