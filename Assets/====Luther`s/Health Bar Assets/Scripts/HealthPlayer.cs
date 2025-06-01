using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
//using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthPlayer : MonoBehaviour
{
    public Image healthBar;
    public Image energyBar;
    public Image whiteBar;
    public float Maxhealth = 100f;
    public float MaxEnergy = 100f;
    public float healAmp = 1.2f;
    public float healthAmount;
    public float energyAmount;
    private float whiteAmount;
    public float debilitateDegenValue = 1f;
    public float healthRegenValue = 0.1f;

    private float Whathit = 1f;
    public float MegaDM = 75f;
    public float SpikeDM = 30f;
    public float SmallDM = 10f;
    public float RockDM = 100f;
    public float BigDM = 40f;
    public float LavaDM = 0.3f;
    public float BossLavaDM = 0.5f;
    public float SpikeCD = 3f;
    public bool canRegen = true;
    public bool vulnerable = true;
    private bool isRegen = false;
    private Coroutine regenCoroutine;
    public Rigidbody2D playerRB;
    private float kncockbackForce = 50f;
    private float yMultyplayer;
    private float xMultyplayer;
    private float defaultKnock;
    private bool whiteVis;
    public float delayDeathUi = 0f;
    public GameObject deathMenu;
    public Animator pAnimator;
    public UnityEngine.Vector2 knockbackDirection = new UnityEngine.Vector2(-1,1);

    //private bool isDead = false;
    private bool hasPlayedDeathAnim = false;
    private bool Kebals = false;


    void Start()
    {
        Whathit = 1f;
        deathMenu.SetActive(false);
        healthAmount = Maxhealth;
        energyAmount = MaxEnergy;
        whiteAmount = Maxhealth;
        defaultKnock = kncockbackForce;
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if (healthAmount <= 0)
        {
            Debug.LogError("PLAYER DIED , DELAY BEFORE UI");
            canRegen = false;
            isRegen = false;
            if (regenCoroutine != null) StopCoroutine(regenCoroutine);
            if (!hasPlayedDeathAnim)
            {
                hasPlayedDeathAnim = true;
                Debug.Log("Mati");
                pAnimator.SetTrigger("is_PDie");
                //EventCallBack.RubahOrtho(15f, 2f);
                StartCoroutine(CountDownBeforeDies());
            }
            StartCoroutine(CountDownBeforeDies());
        }

        if (canRegen && !isRegen && healthAmount < Maxhealth)
        {
            regenCoroutine = StartCoroutine(NaturalRegen());
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            vulnerable = false;
            TakeDamage(10);
            //Debilitating(10);
            //Debug.LogWarning("Took 10 Damage");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Healing(10);
            Rejuvenating(10);
            WhiteUpdate();
            //EnergyDeplete();
            //Debug.LogWarning("Healed 10 Points");
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (Input.GetKey(KeyCode.I) && healthAmount != Maxhealth && healthAmount! >= 0)
        {
            if (energyAmount > 0)
            {
                Debilitating(debilitateDegenValue);
                Healing(debilitateDegenValue * healAmp);  
            }

            // if (energyAmount <= 0)
            // {
            //     return;
            // }
        }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region Methods
    
    public void TakeDamage(float damage)
    {

        //Debug.Log("vul" + vulnerable);
        //Debug.Log("kebal" + Kebals);
        if (Kebals) return;
        //CinemachineShake.Instance.ShakeCamera(3f, 0.2f);
        //EventCallBack.HitStop();

        //EventCallBack.OnAttack();
        StartCoroutine(LateWhiteUpdate());
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

        //whiteAmount = healthAmount;
        //whiteBar.fillAmount = healthBar.fillAmount;
    }

    public void Rejuvenating(float energyRejuAmount)
    {
        energyAmount += energyRejuAmount;
        energyAmount = Mathf.Clamp(energyAmount, 0,MaxEnergy);
        energyBar.fillAmount = energyAmount / MaxEnergy;
        
    }

    public void Debilitating(float debilitate)
    {
        energyAmount -= debilitate;
        energyBar.fillAmount = energyAmount / MaxEnergy;        
    }

    public void EnergyFulling()
    {;
        energyAmount = MaxEnergy; 
        energyBar.fillAmount = energyAmount / MaxEnergy;  
    }

    private void WhiteUpdate()
    {
        float whiteSpeed = 0.8f;
        while (whiteBar.fillAmount > healthBar.fillAmount)
        {
            whiteBar.fillAmount = Mathf.MoveTowards(whiteBar.fillAmount,healthBar.fillAmount,whiteSpeed * Time.deltaTime);
            whiteVis = false;
            return;
        }
    }

    public void Blood4Energy()
    {
        StartCoroutine(B4ECD());
    }

    bool hangOn = false ;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Enumerator

    IEnumerator B4ECD()
    {
        if(hangOn)yield break;
        hangOn = true;
        Rejuvenating(10);
        yield return new WaitForSeconds(0.5f);
        hangOn = false;
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
            whiteAmount = healthAmount;
            whiteBar.fillAmount = healthBar.fillAmount;
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

    IEnumerator LateWhiteUpdate()
    {
        if(whiteVis)yield break;
        whiteVis = true;
        yield return new WaitForSeconds(0.5f);

        float whiteSpeed = 0.3f;
        while (whiteBar.fillAmount > healthBar.fillAmount)
        {
            whiteBar.fillAmount = Mathf.MoveTowards(whiteBar.fillAmount,healthBar.fillAmount,whiteSpeed * Time.deltaTime);
            whiteVis = false;
            yield return null;
        }
        //WhiteUpdate();
        
    }

    IEnumerator CountDownBeforeDies()
    {
        yield return new WaitForSeconds(delayDeathUi);
        //Scenemanager goes here
        Debug.LogError("OI BIB");
    }

    #endregion
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #endregion
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Kncockback

    private void GiveKnockback(UnityEngine.Vector2 sourcePos)
    {
        UnityEngine.Vector2 direction = ((UnityEngine.Vector2)transform.position - sourcePos).normalized;

        direction.y = yMultyplayer;
        direction.x = xMultyplayer;
        direction.Normalize();
        playerRB.velocity = new UnityEngine.Vector2(0f, 0f);
        playerRB.AddForce(direction * kncockbackForce, ForceMode2D.Impulse);
        yMultyplayer = 0f;
        xMultyplayer = 1f;
    }
#endregion
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Registator
    private void OnTriggerStay2D(Collider2D other)
    {
        if (vulnerable && other.CompareTag("SpikeHB"))
        {
            yMultyplayer = 1f;
            xMultyplayer = 0f;
            vulnerable = false;
            kncockbackForce = 10 - (-50f + healthAmount);
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
            kncockbackForce = defaultKnock;
            GiveKnockback(other.transform.position);
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

        if (vulnerable && other.CompareTag("RockATKHB"))
        {
            vulnerable = false;
            TakeDamage(RockDM);
        
            StartCoroutine(InvulnerableCD());
        }

        if (other.CompareTag("LavaHB"))
        {
            TakeDamage(LavaDM);
        }

        if (other.CompareTag("BossLavaATKHB"))
        {
            TakeDamage(BossLavaDM);
        }

        if (energyAmount != MaxEnergy && other.CompareTag("Finish"))
        {
            Rejuvenating(0.7f);
        }
    }
    #endregion
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

    private void isDeads()
    {
        //CountDown();
    }
    private void Kebal()
    {
        StartCoroutine(Kbal());
    }

    private IEnumerator Kbal()
    {
        Kebals = true;
        yield return new WaitForSeconds(0.5f);
        Kebals = false;
    }

    private void OnEnable()
    {
        EventCallBack.Load += LoadHealth;
        EventCallBack.DeadNigga += isDeads;
        EventCallBack.Kebal += Kebal;
    }

    private void OnDisable()
    {
        EventCallBack.Load -= LoadHealth;
        EventCallBack.DeadNigga -= isDeads;
        EventCallBack.Kebal -= Kebal;
    }
}