using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
