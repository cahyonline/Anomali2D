using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyGiantCutscene : MonoBehaviour
{
    public PlayableDirector giantCutschene;
    public GameObject RootGameobject;
    //public GameObject CanvasRender;
    public GameObject invisibleWall;
    //public GameObject intereactEUI;
    private bool activeCutschene = false;


    // Start is called before the first frame update
    void Start()
    {
        RootGameobject.SetActive(true);
        //CanvasRender.SetActive(false);
        invisibleWall.SetActive(true);
        //intereactEUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activeCutschene)
        {
            activeCutschene = true;
            Debug.Log("detect");
            giantCutschene.Play();
        }
    }

    public void DestroyThis()
    {
        RootGameobject.SetActive(false);
        invisibleWall.SetActive(false);
        //Debug.Log("Ended");
    }
}
