using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject pauseMenu;
    public enum GameState { Level_Select, Entering, Level, Leaving }
    public static GameState _currentState;

    public bool levelRunning;
    public bool levelWon;
    public bool isPaused;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                Unpause();
        }
    }

    public void LevelWin()
    {
        levelWon = true;
        levelRunning = false;
    }

    public void Pause()
    {
        //time scale 0
        isPaused = true;
        Time.timeScale = 0;

        //Enable pause menu
        pauseMenu.SetActive(true);
    }

    public void Unpause()
    {
        //time scale 1
        isPaused = false;
        Time.timeScale = 1;

        //disable pause menu
        pauseMenu.SetActive(false);
    }

    public void RestartCheckpoint()
    {
        Unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoadMenu()
    {
        Unpause();
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
