using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hehehe : MonoBehaviour
{
    [SerializeField] private bool hehe;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hehe = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hehe = false;
        }
    }

    private void Update()
    {
        if (hehe)
        {
            EventCallBack.ChangeArea(11);
            EventCallBack.ChangeAreaSpawn(50);
            EventCallBack.Vignette();
            hehe = false;
        }
    }
}
