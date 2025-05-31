using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class CutscenePlayer3 : MonoBehaviour
{
////////////////////////////////////////////////////////////////
/// 
    public PlayableDirector Cutscene3;
    //public testScript playerScript;
    public GameObject theTrigger;
    public GameObject NPC;
    ////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Cutscene1 Trigger Ready");
        NPC.SetActive(true);
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
            //Debug.Log("Detected");
            Cutscene3.Play();
            GamesState.InCutscene = true;
            EventCallBack.OnAttack();
            //theTrigger.SetActive(false);
            //playerScript.enabled = false;
        }
    }

    public void CutsceneOver()
    {
        //playerScript.enabled = true;
        theTrigger.SetActive(false);
        Debug.Log("Cutscene Ended");
        GamesState.InCutscene = false;
        EventCallBack.EndAttack();
        NPC.SetActive(false);
    }

    public void PauseCutscene()
    {
        Cutscene3.Pause();
    }
    public void ResumeCutscene()
    {
        Cutscene3.Play();
    }

}
