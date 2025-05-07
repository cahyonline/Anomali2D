using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{
    public Image healthBar;
    public float Maxhealth = 100f;
    public float healthAmount;
    public float healthRegenValue = 0.1f;
    public float healthRegenRate = 0f;
    private float Whathit = 1f;
    public float MegaDM = 75;
    public float SpikeDM = 30f;
    public float SmallDM = 10f;
    public float BigDM = 40f;
    public float LavaDM = 0.3f;
    public float SpikeCD = 3f;
    public bool canRegen = true;
    public bool vulnerable = true;
    private bool isRegen = false;
    private Coroutine regenCoroutine;
    public Rigidbody2D playerRB;
    private float kncockbackForce;
    private float defaultKnock;    
    public float delayDeathUi = 5f;
    public GameObject deathMenu;
    public Animator pAnimator;
    public UnityEngine.Vector2 knockbackDirection = new UnityEngine.Vector2(-1,1);

    void Start()
    {
        Whathit = 1f;
        deathMenu.SetActive(false);
        healthAmount = Maxhealth;
        defaultKnock = kncockbackForce;
        //kncockbackForce = SpikeDM * 5 - (110f + healthAmount);
    }

    void Update()
    {
        if (healthAmount <= 0)
        {
            Debug.LogError("PLAYER DIED , DELAY BEFORE UI");
            canRegen = false;
            isRegen = false;
            if (regenCoroutine != null) StopCoroutine(regenCoroutine);
            StartCoroutine(CountDown());
        }

        if (canRegen && !isRegen && healthAmount < Maxhealth)
        {
            regenCoroutine = StartCoroutine(NaturalRegen());
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TakeDamage(10);
            //Debug.LogWarning("Took 10 Damage");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Healing(10);
            //Debug.LogWarning("Healed 10 Points");
        }
    }

    public void TakeDamage(float damage)
    {
        pAnimator.SetTrigger("is_PHurt");
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / Maxhealth;
        canRegen = false;
        isRegen = false;

        if (regenCoroutine != null) StopCoroutine(regenCoroutine);
        StartCoroutine(NaturalRegenCD());
    }

    public void Healing(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, Maxhealth);
        healthBar.fillAmount = healthAmount / Maxhealth;

        if (healthAmount >= Maxhealth)
        {
            isRegen = false;
            if (regenCoroutine != null) StopCoroutine(regenCoroutine);
        }
    }

    IEnumerator NaturalRegenCD()
    {
        yield return new WaitForSeconds(5f);
        kncockbackForce = defaultKnock;
        canRegen = true;
    }

    IEnumerator NaturalRegen()
    {
        isRegen = true;

        while (canRegen && healthAmount < Maxhealth)
        {
            yield return new WaitForSeconds(1f);
            healthAmount += healthRegenValue;
            healthAmount = Mathf.Clamp(healthAmount, 0, Maxhealth);
            healthBar.fillAmount = healthAmount / Maxhealth;

            if (healthAmount >= Maxhealth)
            {
                isRegen = false;
                yield break;
            }
        }

        isRegen = false;
    }

    private void GiveKnockback(UnityEngine.Vector2 sourcePos)
    {
        UnityEngine.Vector2 direction = ((UnityEngine.Vector2)transform.position - sourcePos).normalized;

        direction.y = 0f;
        direction.Normalize();
        playerRB.velocity = new UnityEngine.Vector2(0f, 0f);
        playerRB.AddForce(direction * kncockbackForce,ForceMode2D.Impulse);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (vulnerable && other.CompareTag("SpikeHB"))
        {
            vulnerable = false;
            kncockbackForce = SpikeDM * 2 - (-110f + healthAmount);
            //Debug.LogWarning(kncockbackForce);
            Whathit = SpikeCD;
            TakeDamage(SpikeDM);
            GiveKnockback(other.transform.position);
            StartCoroutine(InvulnerableCD());
        }

        if (vulnerable && other.CompareTag("SmallATKHB"))
        {
            vulnerable = false;
            TakeDamage(SmallDM);
            StartCoroutine(InvulnerableCD());
        }

        if (vulnerable && other.CompareTag("BigATKHB"))
        {
            vulnerable = false;
            TakeDamage(BigDM);
            StartCoroutine(InvulnerableCD());
        }

        if (vulnerable && other.CompareTag("MegaATKHB"))
        {
            vulnerable = false;
            kncockbackForce = MegaDM * 5 - (-210f + healthAmount);
            GiveKnockback(other.transform.position);
            TakeDamage(MegaDM);
            StartCoroutine(InvulnerableCD());
        }

        if (other.CompareTag("LavaHB"))
        {
            TakeDamage(LavaDM);
        }
    }

    private IEnumerator InvulnerableCD()
    {
        yield return new WaitForSeconds(Whathit);
        vulnerable = true;
        Whathit = 1f;
        //Debug.LogWarning("vulnerable");
    }

    private void LoadHealth(SaveData healthbarre)
    {
        healthAmount = healthbarre.HealthInfor;
        healthBar.fillAmount = healthAmount / 100f;
    }

    private void OnEnable()
    {
        EventCallBack.Load += LoadHealth;
    }

    private void OnDisable()
    {
        EventCallBack.Load -= LoadHealth;
    }

    private IEnumerator CountDown()
    {
        pAnimator.SetTrigger("is_PDie");
        yield return new WaitForSeconds(delayDeathUi);
        deathMenu.SetActive(true);
    }
}