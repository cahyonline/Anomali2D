using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class isUnlock1: MonoBehaviour
{
    //[SerializeField] private PlayerInventory inventory;
    [SerializeField] private bool isUnlocks = false;
    [SerializeField] private bool Unlock1 = false;
    //[SerializeField] private Animator animss;
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
        if ( Input.GetKeyDown(KeyCode.E) && isUnlocks)
        {
            StartCoroutine(Meletakkan());
            //isUnlocks = false;
            UIpickupE.SetActive(false);
        }
    }

    private IEnumerator Meletakkan()
    {
        if (isUnlocks)
        {
            PlayerAnimator.MasukGoa = true;
            yield return new WaitForSeconds(1);
           // Debug.Log("Unlock1");
            EventCallBack.ChangeArea(17);
            EventCallBack.ChangeAreaSpawn(37);
            EventCallBack.Vignette();
        }
    }
}
