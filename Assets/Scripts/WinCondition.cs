using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    // Start is called before the first frame update

    public SceneManagerer sceneManagerer;
    public bool canPickup;
    public bool WinGame = false;
    void Start()
    {
        canPickup = false;
        WinGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPickup && Input.GetKey(KeyCode.E))
        {
            canPickup = false;
            sceneManagerer.LoadMainMenu();
        }
        if (WinGame)
        {
            sceneManagerer.LoadMainMenu();
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            //Debug.Log("Innit");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
            //Debug.Log("OUSIDE");
        }
    }
    
}
