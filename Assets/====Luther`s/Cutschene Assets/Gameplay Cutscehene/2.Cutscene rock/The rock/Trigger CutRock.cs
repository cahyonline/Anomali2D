using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutrock : MonoBehaviour
{
    public PlayableDirector cutsceneRock;
    public GameObject RootGameobject;
    public GameObject CanvasRender;
    public GameObject invisibleWall;
    public GameObject intereactEUI;
    [SerializeField] private PlayerAnimator playerAnimator;
    private bool activeCutschene = false;


    // Start is called before the first frame update
    void Start()
    {
        RootGameobject.SetActive(true);
        CanvasRender.SetActive(false);
        invisibleWall.SetActive(true);
        intereactEUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && !activeCutschene)
            {
                activeCutschene = true;
                CanvasRender.SetActive(true);
                cutsceneRock.Play();
                playerAnimator.InteractE2 = true;
                GamesState.InCutscene = true;
                EventCallBack.OnAttack();
            }

            intereactEUI.SetActive(true);
        }
    }

    public void DestroyThis()
    {
        RootGameobject.SetActive(false);
        invisibleWall.SetActive(false);
        

        //Debug.Log("Ended");
    }

    public void PlayerCanMove()
    {
        GamesState.InCutscene = false;
        EventCallBack.EndAttack();
        //Debug.LogWarning("Aaaaaaaaaaaaaaaaaaaaaaaaaa");
    }
}
