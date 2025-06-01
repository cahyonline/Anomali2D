using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class IsLetakkan : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private bool isLetakkan = false;
    [SerializeField] private Animator animss;
    [SerializeField] private GameObject UIpickupE;
    [SerializeField] private bool EInteract;
    [SerializeField] private PlayerAnimator PlayerAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& inventory.HasItem(ItemType.Fragment))
        {
            isLetakkan = true;
            UIpickupE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && inventory.HasItem(ItemType.Fragment))
        {
            isLetakkan = false;
            UIpickupE.SetActive(false);
        }
    }

    private void Update()
    {
        if (GamesState.InInteract) return;
        if (GamesState.InCutscene) return;
        if (isLetakkan && Input.GetKeyDown(KeyCode.E))
        {
            Meletakkan();
            isLetakkan = false;
            UIpickupE.SetActive (false);
        } 
    }

    private void Meletakkan()
    {
        animss.SetTrigger("isLetakkan");
        PlayerAnimator.InteractE = true;
    }

    private void isDoneAnim()
    {
        EventCallBack.ChangeArea(21);
        EventCallBack.ChangeAreaSpawn(45);
        EventCallBack.Vignette();
    }

}
