using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class isUnlock: MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private bool isUnlocks = false;
    [SerializeField] private bool Unlock = false;
    [SerializeField] private Animator animss;
    [SerializeField] private GameObject UIpickupE;
    [SerializeField] private bool EInteract;
    [SerializeField] private PlayerAnimator PlayerAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            isUnlocks = true;
            UIpickupE.SetActive(true);
            //Debug.Log("kont");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")  )
        {
            isUnlocks = false;
            UIpickupE.SetActive(false);
        }
    }

    private void Update()
    {
        if (GamesState.InInteract) return;
        if (GamesState.InCutscene) return;
        if (GamesState.InInteractCheckpoint) return;

        if ( Input.GetKeyDown(KeyCode.E) && isUnlocks)
        {
            StartCoroutine(Meletakkan());
            //isUnlocks = false;
            UIpickupE.SetActive(false);
        }
    }

    private IEnumerator Meletakkan()
    {
        if (Unlock)
        {
            PlayerAnimator.MasukGoa = true;
            yield return new WaitForSeconds(1);
            //Debug.Log("Unlock");
            EventCallBack.ChangeArea(18);
            EventCallBack.ChangeAreaSpawn(39);
            EventCallBack.Vignette();
        }
        else 
        {
            animss.SetTrigger("isUnlock");
            PlayerAnimator.InteractE2 = true;
            Unlock = true;
        }
    }
}
