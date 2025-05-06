using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenus : MonoBehaviour
{
    public void StartStory()
    {
        SceneManager.LoadScene("Games");
    }

    public void QuitGames()
    {
        Application.Quit();
    }
}
