using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Scripting; 

[Preserve] 
public class IdleCombatState : State
{
    public void Start()
    {
        EventCallBack.EndAttack();
    }
}
