using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCharacter : MonoBehaviour
{

    private StateMachine meleeStateMachine;

    [SerializeField] public Collider2D hitbox;
    [SerializeField] public GameObject Hiteffect;

    void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
        {
            EventCallBack.OnAttack();
            meleeStateMachine.SetNextState(new GroundEntryState());
            
        }
    }
    private void PlaySfxStep()
    {
        AudioManager.Instance.PlaySFX("Step");
    }
}
