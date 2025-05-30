using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItem itemData; 
    public int amount = 1;
    [SerializeField] private GameObject UIpickupE;
    [SerializeField] private bool EInteractTerang;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerAnimator animHandler;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && itemData != null)
        {
            EInteractTerang = true;
            UIpickupE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EInteractTerang = false;
            UIpickupE.SetActive(false);
        }
    }

    private void Update()
    {
        if( GamesState.InCutscene ) return;
        if (EInteractTerang && Input.GetKeyDown(KeyCode.E))
        {
            isTerang(); // Pick up item
            EInteractTerang = false;
            UIpickupE.SetActive(false);
        }
    }

    private void isTerang()
    {
        //PlayerInventory inventory = GetComponent<PlayerInventory>();

        if (inventory != null && itemData != null)
        {
            animHandler.InteractE = true;
            inventory.AddItem(itemData, amount);
            Debug.Log($"Player mengambil item: {itemData.itemName} x{amount}");
            StartCoroutine(destroy());
        }
    }
    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
    }

}
