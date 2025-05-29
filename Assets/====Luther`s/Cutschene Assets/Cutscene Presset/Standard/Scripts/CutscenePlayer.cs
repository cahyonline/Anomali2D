using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class CutscenePlayer : MonoBehaviour
{
////////////////////////////////////////////////////////////////
/// 
    public PlayableDirector Cutscene1;
    //public testScript playerScript;
    public GameObject theTrigger;
////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Cutscene1 Trigger Ready");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
////////////////////////////////////////////////////////////////
/// 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Detected");
            Cutscene1.Play();
            theTrigger.SetActive(false);
            //playerScript.enabled = false;
        }
    }

    public void CutsceneOver()
    {
        //playerScript.enabled = true;
        theTrigger.SetActive(false);
        Debug.Log("Cutscene Ended");
    }

    public void PauseCutscene()
    {
        Cutscene1.Pause();
    }
    public void ResumeCutscene()
    {
        Cutscene1.Play();
    }

}
