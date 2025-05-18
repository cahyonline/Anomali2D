using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealTest : MonoBehaviour
{

    public HealthPlayer healthPlayer;
    private bool hangOn;

    // Start is called before the first frame update
    void Start()
    {
        hangOn = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D thisThing)
    {
        if (thisThing.CompareTag("Enemy"))
        {
            //StartCoroutine(LifeSteal());
            healthPlayer.Blood4Energy();
            //Debug.Log("register");
        }
    }

    // IEnumerator LifeSteal()
    // {
    //     if (hangOn) yield break;
    //     Debug.Log("register");
    //     hangOn = true;
    //     yield return new WaitForSeconds(0.1f);

    //     hangOn = false;
    // }
}
