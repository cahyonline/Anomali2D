using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxStone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("PlayerDamage"))
        {
            
            AudioManager.Instance.SFXaddOn("stoned");
            Debug.Log("Stoned");


        }
    }
}
