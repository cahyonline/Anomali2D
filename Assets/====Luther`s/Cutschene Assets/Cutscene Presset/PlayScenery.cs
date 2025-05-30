using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScenery : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Debugger, Debugger2;
    void Start()
    {
        Debugger.SetActive(false);
    }

    public void ShowDebugger()
    {
        Debugger.SetActive(true);
        //Debug.LogWarning("TRIGGERED");
    }

    public void ShowDebugger2()
    {
        Debugger2.SetActive(true);
    }

    public void HideDebugger()
    {
        Debugger.SetActive(false);
    }
}
