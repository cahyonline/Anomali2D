using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class HealthEnemy : MonoBehaviour
{
///////////////////////////////////////////////////////////////////
    public Image healthBar;
    public float healthAmount = 100f;
    private float Whathit = 0.1f;
    public float SpikeDM = 20f;
    public float SmallDM = 10f;
    public float BigDM = 40f;
    public float LavaDM = 0.3f;
    public float PlayerDamage = 35f;
    public float SpikeCD = 3f;
    public bool vulnerable = true;
    //private bool isBlockedHere;
    [Header("IMPORTANT")]
    public GameObject EnemyRootObject;
    public EnemyAIMelee enemyAIMelee;
///////////////////////////////////////////////////////////////////
    void Start()
    {
        //Whathit = 0f;
        //enemyAIMelee = GetComponent<EnemyAIMelee>();
    }

    // Update is called once per frame
///////////////////////////////////////////////////////////////////
    void Update()
    {
        if(healthAmount <=0)
        {
            Debug.LogError("Enemy DIED");
            //Destroy(EnemyRootObject);
        }

        if (EnemyAIMelee.isBlocked == true)
        {
            //isBlockedHere = true;
            Debug.LogWarning("Denied");
        }
        if(EnemyAIMelee.isBlocked == false)
        {
            //isBlockedHere = false;
            Debug.LogWarning("Open");
        }

///////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////
    }
///////////////////////////////////////////////////////////////////
///// DAMAGE REGISTER
        public void TakeDamage (float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 200f;
    }
    public void Healing (float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 200f;
    }
///////////////////////////////////////////////////////////////////
////// DAMAGE REGISTER HITBOX
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(vulnerable && other.CompareTag("PlayerDamage") && EnemyAIMelee.isBlocked)
        {
            vulnerable = false;
            //Whathit = SpikeCD;
            TakeDamage(PlayerDamage);
            StartCoroutine(InvulnerableCD());
        }
    }
///////////////////////////////////////////////////////////////////
////// DAMAGE COOLDOWN
    private IEnumerator InvulnerableCD()
    {
        yield return new WaitForSeconds(Whathit);
        vulnerable = true;
        //Whathit = 0f;
        Debug.LogWarning("vulnerable");
    }
}
