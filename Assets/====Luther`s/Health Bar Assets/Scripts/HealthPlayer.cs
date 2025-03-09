using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class HealthPlayer : MonoBehaviour
{
///////////////////////////////////////////////////////////////////
    public Image healthBar;
    public float healthAmount = 100f;
    public float healthRegenValue = 0.1f;
    public float healthRegenRate = 0f;
    private float Whathit = 1f;
    public float SpikeDM = 30f;
    public float SmallDM = 10f;
    public float BigDM = 40f;
    public float LavaDM = 0.3f;
    public float SpikeCD = 3f;
    public bool canRegen = true;
    public bool vulnerable = true;
    private bool isRegen = true;
///////////////////////////////////////////////////////////////////
void Start()
{
    Whathit = 1f;
}

///////////////////////////////////////////////////////////////////
void Update()
{
    if (healthAmount <= 0)
    {
        Debug.LogError("PLAYER DIED");
        canRegen = false;
        isRegen = false;
    }

    if (canRegen && !isRegen) // Only start regen if it's NOT already running
    {
        StartCoroutine(NaturalRegen());
    }

///////////////////////////////////////////////////////////////////
/// DEBUG DAMAGE TAKEN
    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
        TakeDamage(10);
        Debug.LogWarning("Took 10 Damage");
    }

    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
        Healing(10);
        Debug.LogWarning("Healed 10 Points");
    }
}

///////////////////////////////////////////////////////////////////
/// DAMAGE REGISTER
public void TakeDamage(float damage)
{
    healthAmount -= damage;
    healthBar.fillAmount = healthAmount / 100f;
    canRegen = false;
    isRegen = false; 
    StartCoroutine(NaturalRegenCD());
}

public void Healing(float healAmount)
{
    healthAmount += healAmount;
    healthAmount = Mathf.Clamp(healthAmount, 0, 100);
    healthBar.fillAmount = healthAmount / 100f;
    if(healthAmount >= 100f)
    {
        isRegen = false;
    }
}

IEnumerator NaturalRegenCD()
{
    yield return new WaitForSeconds(5f);
    canRegen = true;
}

IEnumerator NaturalRegen()
{
    isRegen = true; 

    while (canRegen)
    {
        yield return new WaitForSeconds(1);
        healthAmount += healthRegenValue;
        healthBar.fillAmount = healthAmount / 100f;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        
        //if(healthAmount >= 100f)
        //{
            //isRegen = false;
        //}
    }

    isRegen = false; 
}

///////////////////////////////////////////////////////////////////
////// DAMAGE REGISTER HITBOX
    private void OnTriggerStay2D(Collider2D other)
    {
        if(vulnerable && other.CompareTag("SpikeHB"))
        {
            vulnerable = false;
            Whathit = SpikeCD;
            TakeDamage(SpikeDM);
            StartCoroutine(InvulnerableCD());
        }

        if(vulnerable && other.CompareTag("SmallATKHB"))
        {
            vulnerable = false;
            TakeDamage(SmallDM);
            StartCoroutine(InvulnerableCD());
        }

        if(vulnerable && other.CompareTag("BigATKHB"))
        {
            vulnerable = false;
            TakeDamage(BigDM);
            StartCoroutine(InvulnerableCD());
        }
        if(vulnerable && other.CompareTag("LavaHB")) //need to keep updating object
        {
            vulnerable = true;
            TakeDamage(LavaDM);
            //StartCoroutine(InvulnerableCD());
        }
    }
///////////////////////////////////////////////////////////////////
////// DAMAGE COOLDOWN
    private IEnumerator InvulnerableCD()
    {
        yield return new WaitForSeconds(Whathit);
        vulnerable = true;
        Whathit = 1f;
        Debug.LogWarning("vulnerable");
    }
}
