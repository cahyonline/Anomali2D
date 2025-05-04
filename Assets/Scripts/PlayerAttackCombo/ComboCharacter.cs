using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSurface;

public class ComboCharacter : MonoBehaviour
{

    private StateMachine meleeStateMachine;

    [SerializeField] public Collider2D hitbox;
    [SerializeField] public GameObject Hiteffect;

    void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
        surfaceDetector = GetComponent<PlayerSurface>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
        {
            EventCallBack.OnAttack();
            meleeStateMachine.SetNextState(new GroundEntryState());
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private PlayerSurface surfaceDetector;

    private void PlaySfxStep()
    {
        if (surfaceDetector == null) return;

        switch (surfaceDetector.currentSurface)
        {
            case SurfaceType.Tanah:
                AudioManager.Instance.PlaySFX("StepTanah");
                //Debug.Log("Tanah");
                break;
            case SurfaceType.Batu:
                AudioManager.Instance.PlaySFX("StepBatu");
                //Debug.Log("Batu");
                break;
        }
    }

}
