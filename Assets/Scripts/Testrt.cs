using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testrt : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDamage"))
        {
            //Debug.Log("PlayerDamage");
            EventCallBack.HitStop.Invoke();
            //
        }
    }
}
