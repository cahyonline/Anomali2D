using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class CutscenePlayer2 : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////
    /// 
    public PlayableDirector StarterCutscene;
    //public testScript playerScript;
    public GameObject fade;

    public GameObject CutSceneGameRoot;
    [SerializeField] private GameObject playerGoesHere;
    ////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Cutscene1 Trigger Ready");
        CutSceneGameRoot.SetActive(true);
        fade.SetActive(true);
        PlayCutscene();
        playerGoesHere.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
    ////////////////////////////////////////////////////////////////
    /// 
    public void PlayCutscene()
    {
        StarterCutscene.Play();
    }
    public void CutsceneOver()
    {
        //playerScript.enabled = true;
        //theTrigger.SetActive(false);
        StartCoroutine(TimerDestroy());
        
        //Debug.Log("Cutscene Ended");
    }

    public void CutsceneInstantOver()
    {
        CutSceneGameRoot.SetActive(false);
        playerGoesHere.SetActive(true);
    }

    public void PauseCutscene()
    {
        StarterCutscene.Pause();
    }
    public void ResumeCutscene()
    {
        StarterCutscene.Play();
    }

    public IEnumerator TimerDestroy()
    {
        yield return new WaitForSeconds(5);
        CutSceneGameRoot.SetActive(false);
    }
}
