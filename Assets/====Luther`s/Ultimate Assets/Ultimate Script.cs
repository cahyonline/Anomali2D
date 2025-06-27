using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class UltimateScript : MonoBehaviour
{
    public UnityEngine.UI.Image ultBar;
    //public UnityEngine.UI.Image whiteBar;
    public GameObject playerBigDamage;
    public BoxCollider2D playerDefaultDamage;
    public float currentUlt = 0f;
    public float timerToFill = 60f;
    private bool whiteVis;
    private float MaxUlt = 100f;
    private float defaultMaxUlt;
    private float percentage = 0.90f;
    //private float durationPercentage;
    private bool ultAvailable = false;
    private bool ultimateOn = false;
    private bool attackingPG = false;
    public bool isHaveUlt = false;
    private bool isUltimates = false;
    private UnityEngine.Vector2 originalBoxSize;

    [SerializeField] private PlayerAnimator playerAnimator;

    //public float ultDuration;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        defaultMaxUlt = MaxUlt;
        currentUlt = 100f;
        //whiteBar.fillAmount = currentUlt;
        defaultMaxUlt = defaultMaxUlt * percentage;
        //durationPercentage = 1f / ultDuration;
        playerBigDamage.SetActive(false);
        originalBoxSize = playerDefaultDamage.size;
        isHaveUlt = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < timerToFill && currentUlt <= MaxUlt && !ultimateOn && isHaveUlt)
        {
            timer += Time.deltaTime;
            float fillerAmount = timer / timerToFill;
            RegainUlt((MaxUlt * fillerAmount) - currentUlt);
            //StartCoroutine(LateWhiteUpdate());
        }

        // if (currentUlt >= MaxUlt)
        // {
        //     //whiteBar.fillAmount = currentUlt;
        // }

        if (currentUlt >= MaxUlt)
        {
            //Debug.Log("ready");
            ultAvailable = true;
            isUltimates = false;

            //Debug.Log(isUltimates);
        }

        if (Input.GetKeyDown(KeyCode.U) && ultAvailable)
        {
            UseUlt();
        }

        if (!isHaveUlt)
        {
            currentUlt = 0f;
        }

        // if (Input.GetKeyDown(KeyCode.DownArrow))
        //     {
        //         UseUlt();
        //     }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RegainUlt(10);
        }

        // if (ultimateOn && !attackingPG)
        // {
        //     if (Input.GetKeyDown(KeyCode.J))
        //     {
        //         StartCoroutine(Attacking());
        //     }
        // }

        // if (ultimateOn)
        // {
        //     playerDefaultDamage.enabled = false;
        // }
    }



    public void RegainUlt(float Regaining)
    {
        currentUlt += Regaining;
        currentUlt = Mathf.Clamp(currentUlt, 0, MaxUlt);
        ultBar.fillAmount = currentUlt / MaxUlt;
    }

    public void DegenUlt(float Degen)
    {
        currentUlt -= Degen;
        currentUlt = Mathf.Clamp(currentUlt, 0, MaxUlt);
        ultBar.fillAmount = currentUlt / MaxUlt;
        //ultBar.fillAmount = Mathf.MoveTowards(ultBar.fillAmount, 0f, 0.5f * Time.deltaTime);

    }
    public void UseUlt()
    {
        if (currentUlt == MaxUlt && ultAvailable)
        {

            ultAvailable = false;
            ultimateOn = true;
            currentUlt = 0f;
            timer = 0f;
            //StartCoroutine(OnUltimateSkillTimer());
            OnUltimateSkillTimer();
            StartCoroutine(Attacking());
            DegenUlt(MaxUlt);
            //StartCoroutine(LateWhiteUpdate());
        }

    }

    public void OnUltimateSkillTimer()
    {
        //Debug.Log("Ulting");
        playerDefaultDamage.size = UnityEngine.Vector2.zero;

        playerDefaultDamage.size = originalBoxSize;
        ultimateOn = false;
        //Debug.Log("Done");
    }

    IEnumerator Attacking()
    {
        if (attackingPG) yield break;
        attackingPG = true;
        ////
        if (!isUltimates)
        {
            playerAnimator.Ultimate = true;
            Debug.Log("Kontol");
            isUltimates = true;
        }
        ////
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.SFXaddOn("Ultimate");
        playerBigDamage.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        attackingPG = false;
        playerBigDamage.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (currentUlt <= defaultMaxUlt && other.CompareTag("Finish") && !ultimateOn)
        {
            timer = timer + 0.5f;
            // float whiteSpeed = 3f;
            // while (whiteBar.fillAmount < percentage)
            // {
            //     whiteBar.fillAmount = Mathf.MoveTowards(whiteBar.fillAmount, percentage, whiteSpeed * Time.deltaTime);
            //     return;
            // }
        }
    }

    public void HasUlt()
    {
        isHaveUlt = true;
    }
}
