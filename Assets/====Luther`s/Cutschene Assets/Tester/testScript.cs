using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Debugger;
    void Start()
    {
        Debugger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDebugger()
    {
        Debugger.SetActive(true);
        Debug.LogWarning("TRIGGERED");
    }
}
