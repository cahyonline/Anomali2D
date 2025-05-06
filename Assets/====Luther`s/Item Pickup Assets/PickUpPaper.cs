using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPaper : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject showPaper;
    public GameObject UIpickupE;
    public GameObject UIpickupEnter;
    public bool canPickup;
    public bool interactedPaper;
    public PlayerControl playerControl;
    void Start()
    {
        showPaper.SetActive(false);
        canPickup = false;
        interactedPaper = false;
        UIpickupE.SetActive(false);
        UIpickupEnter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canPickup && Input.GetKey(KeyCode.E))
        {
            showPaper.SetActive(true);
            interactedPaper = true;
            UIpickupE.SetActive(false);
            UIpickupEnter.SetActive(true);
            playerControl.enabled = false;
            canPickup = false;
        }
        if(interactedPaper && Input.GetKey(KeyCode.Return))
        {
            showPaper.SetActive(false);
            canPickup = false;
            UIpickupEnter.SetActive(false);
            playerControl.enabled = true;
            Destroy(gameObject);
            //Debug.LogWarning("DESTROYED");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            canPickup = true;
            UIpickupE.SetActive(true);
            //Debug.Log("Innit");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            canPickup = false;
            UIpickupE.SetActive(false);
            //Debug.Log("OUSIDE");
        }
    }
}
