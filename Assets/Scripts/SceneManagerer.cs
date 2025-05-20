using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerer : MonoBehaviour
{
    private bool isPaused = false;
    private bool canPaused = true;
    public GameObject pausedUI;

    private void Start()
    {
        Time.timeScale = 1f;
        pausedUI.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPaused)
        {
            TogglePause();
            Debug.LogWarning("Toggled");
        }
    }
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// </summary>
    public void LoadLevel1()
    {
        SceneManager.LoadScene("level 1");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //public void LoadScene3()
    //{
        //SceneManager.LoadScene("Scene3");
    //}

    public void QuitGame()
    {
        Application.Quit();
        Debug.LogError("APP QUIT");
    }

    public void PauseGame()
    {
       Time.timeScale = 0f;
       isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            pausedUI.SetActive(false);
            UnpauseGame();
        }
            
        else
        {
            PauseGame();
            pausedUI.SetActive(true);
        }
            
    }
    public void MainMenuz()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadingScene()
    {
        canPaused = !canPaused;
    }
}