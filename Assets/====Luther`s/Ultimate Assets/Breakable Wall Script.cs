using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int howMuchHit = 3;
    private int currentHit = 0;
    void Start()
    {
        currentHit = 0;
    }

    // Update is called once per frame
    public void RegisterHit()
    {
        currentHit++;

        if (currentHit >= howMuchHit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDamageBig"))
        {
            RegisterHit();
            Debug.Log("HIT");
        }
    }
}
