using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitPillar : MonoBehaviour
{
    //===========================================================
    public Animator animatorP;
    //===========================================================
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerDamage"))
        {
            Debug.LogWarning(" Sword Hit Pillar ");
            animatorP.SetBool("isHit" , true);
        }
    }
}
