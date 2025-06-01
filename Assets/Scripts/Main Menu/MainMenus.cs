using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenus : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject canvasVideo;
    public GameObject CreditPanel;
    public GameObject OptionPanel;
    public ParticleSystem particleSystem;
    public VideoPlayer videoPlayer;
    void Start()
    {
        MenuPanel.SetActive(true);
        CreditPanel.SetActive(false);
        OptionPanel.SetActive(false);
        canvasVideo.SetActive(false);
        // Cek jika di MainMenu, restart partikel
        particleSystem.Play();
        videoPlayer.Stop();
        //if (SceneManager.GetActiveScene().name == "MainMenu")
        //{
        //    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        //}

    }
    public void StartStory()
    {
        //SceneManager.LoadScene("Games");
        canvasVideo.SetActive(true);
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("Games");
    }

    public void LoadGame()
    {
        EventCallBack.LoadGame();
    }

    public void QuitGames()
    {
        Application.Quit();
        Debug.Log("game telah keluar");
    }

    public void CreditButton()
    {
        MenuPanel.SetActive(false);
        CreditPanel.SetActive(true);
        OptionPanel.SetActive(false);
    }
    public void OptionButton()
    {
        MenuPanel.SetActive(false);
        CreditPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void BackButton()
    {
        MenuPanel.SetActive(true);
        CreditPanel.SetActive(false);
        OptionPanel.SetActive(false);
    }
}
