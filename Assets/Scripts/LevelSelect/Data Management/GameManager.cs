using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool levelRunning;
    public bool levelWon;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        levelRunning = true;
        levelWon = false;
    }

    public void LevelWin()
    {
        levelWon = true;
        levelRunning = false;
    }
}
