using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
public class CutscenePlayerBoss : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////
    /// 
    public PlayableDirector StarterCutscene;
    //public testScript playerScript;
    public GameObject fade;
    //public GameObject bossObject;
    public EnemyAIGBoss enemyAIGBoss;
    public GameObject CutSceneGameRoot;
    //[SerializeField] private GameObject playerGoesHere;
    ////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Cutscene1 Trigger Ready");
        CutSceneGameRoot.SetActive(true);
        fade.SetActive(true);

        //PlayCutscene();
        //bossObject.SetActive(false);
        //playerGoesHere.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyAIGBoss.healthAmount <= 0)
        {
            TriggerAtLast();
        }
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
        //CutSceneGameRoot.SetActive(false);
        //bossObject.SetActive(true);
        //playerGoesHere.SetActive(true);
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

    public void PlayerCanMove()
    {
        EventCallBack.EndAttack();
        GamesState.InCutscene = false;
    }

    public void TriggerAtLast()
    {
        Debug.LogError("ENding");
    }
}
