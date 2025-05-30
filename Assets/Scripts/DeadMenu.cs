using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    public void CheckPoint()
    {
        EventCallBack.LoadGame();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game telah Keluar");
    }
}
