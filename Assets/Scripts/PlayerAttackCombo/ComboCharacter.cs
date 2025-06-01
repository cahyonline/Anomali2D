using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSurface;

public class ComboCharacter : MonoBehaviour
{

    [SerializeField] private StateMachine meleeStateMachine;

    [SerializeField] public Collider2D hitbox;
    [SerializeField] public GameObject[] Hiteffect;
    [SerializeField] public PlayerMovement playerMv;

    void Start()
    {
        //meleeStateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {
        if (GamesState.InInteract) return;

        if (Input.GetKeyDown(KeyCode.J))
        {
            //Debug.Log("Key J pressed"); 
            //Debug.Log("Current State: " + meleeStateMachine.CurrentState.GetType()); 
            if (meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
            {
                EventCallBack.OnAttack();
                meleeStateMachine.SetNextState(new GroundEntryState());
            }
        }
    }

    [SerializeField] private PlayerSurface surfaceDetector;

    private void PlaySfxStep()
    {
        if (surfaceDetector == null)
        {
            Debug.Log("null?");
            return;
        }
            switch (surfaceDetector.currentSurface)
        {
            case SurfaceType.Tanah:
                AudioManager.Instance.PlaySFX("StepTanah");
                //Debug.Log("TanahP");x
                break;
            case SurfaceType.Batu:
                AudioManager.Instance.PlaySFX("StepBatu");
                //Debug.Log("BatuP");
                break;
        }
    }

    private void PlaySfxKen()
    {
        AudioManager.Instance.PlaySFX("attackSFX");
        //Debug.Log("Ken");
    }

    private void UIDeathh()
    {
        EventCallBack.DeadNigga();
    }
    private void EndIT()
    {
        GamesState.InInteract = false;
        EventCallBack.EndAttack();
        GamesState.InInteractCheckpoint = false;
       // Debug.Log("ddsdfs");
    }
    private void StrtIt()
    {
        EventCallBack.OnAttack();
    }
}
