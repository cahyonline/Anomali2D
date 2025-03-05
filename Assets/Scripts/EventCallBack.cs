using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventCallBack
{


    #region ITEM
    public static Action CollectItem { get; set; } = delegate { };
    #endregion

    public static Action NextWorld { get; set; } = delegate { };
    public static Action PlayerFalling { get; set; } = delegate { };



}
