using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventCallBack
{
    #region ITEM
    public static Action CollectItem { get; set; } = delegate { };
    #endregion
    public static Action<int> ChangeArea { get; set; } = delegate { };
    public static Action<int> ChangeAreaSpawn { get; set; } = delegate { };
    public static Action <SaveData> Load { get; set; } = delegate { };
    public static Action Vignette { get; set; } = delegate { };
    public static Action PlayerFalling { get; set; } = delegate { };
    public static Action OnAttack { get; set; } = delegate { };
    public static Action EndAttack { get; set; } = delegate { };
    public static Action HitStop { get; set; } = delegate { };

    
}