using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedUlt : MonoBehaviour
{
    [SerializeField] private bool isIlangs = false;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private GameObject Barrier;

    private void Start()
    {
        Barrier.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDamageBig"))
        {
             Barrier.SetActive(false);  
            Destroy(Barrier);
            
        }
    }
}
