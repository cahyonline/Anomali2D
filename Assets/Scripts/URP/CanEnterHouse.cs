using System.Collections;
using UnityEngine;

public class CanEnterHouse : MonoBehaviour
{
    public InventoryItem itemData;
    public int amount = 1;
    [SerializeField] private GameObject UIpickupE;
    [SerializeField] private bool EInteractRumah;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerAnimator animHandler;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && itemData != null && inventory.HasItem(ItemType.CanEnterHouse))
        {
            EInteractRumah = true;
            UIpickupE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && itemData != null && inventory.HasItem(ItemType.CanEnterHouse))
        {
            EInteractRumah = false;
            UIpickupE.SetActive(false);
        }
    }

    private void Update()
    {
        if (GamesState.InCutscene) return;
        if (EInteractRumah && Input.GetKeyDown(KeyCode.E))
        {
            isOPen(); 
            EInteractRumah = false;
            UIpickupE.SetActive(false);
        }
    }

    private void isOPen()
    {
        //PlayerInventory inventory = GetComponent<PlayerInventory>();

        if (inventory != null && itemData != null)
        {
            animHandler.MasukGoa = true;
            StartCoroutine(destroy());
        }
    }
    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(1f);
        EventCallBack.Vignette();
        EventCallBack.ChangeArea(24);
        EventCallBack.ChangeAreaSpawn(48);
        //Destroy(gameObject);
    }

}
