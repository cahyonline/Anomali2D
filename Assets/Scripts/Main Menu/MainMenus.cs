using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Video;

public class MainMenus : MonoBehaviour
{
    //public PlayableDirector startVideo;
    public VideoPlayer ealryVideo;
    public GameObject videoObject;

    void Start()
    {
        videoObject.SetActive(false);
        //ealryVideo.frame = 0;

    }
    public void StartStory()
    {
        //SceneManager.LoadScene("Games");
        //startVideo.Play();
        ealryVideo.frame = 0;
        videoObject.SetActive(true);
        ealryVideo.Play();
        ealryVideo.loopPointReached += OnVideoEnd;
        Debug.Log("start");
        //ealryVideo.Play();

    }
    // public void VideoPrepared(VideoPlayer vp)
    // {
    //     vp.Play();
    // }
    public void OnVideoEnd(VideoPlayer vp)
    {
        videoObject.SetActive(false);
    }

    public void QuitGames()
    {
        Application.Quit();
    }
}
