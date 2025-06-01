using System.Collections;
using UnityEngine;

public class UnlockIt : MonoBehaviour
{
    public InventoryItem itemData;
    public int amount = 1;
    [SerializeField] private GameObject UIpickupE;
    [SerializeField] private GameObject Inirumah;
    [SerializeField] private bool EInteractTerang;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerAnimator animHandler;

    private void Start()
    {
        Inirumah.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && itemData != null && inventory.HasItem(ItemType.KeyItem))
        {
            EInteractTerang = true;
            UIpickupE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && itemData != null && inventory.HasItem(ItemType.KeyItem))
        {
            EInteractTerang = false;
            UIpickupE.SetActive(false);
        }
    }

    private void Update()
    {
        if (GamesState.InCutscene) return;
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
            InventorySlot keySlot = inventory.GetItemByType(ItemType.KeyItem);
            inventory.RemoveItem(keySlot.item, 1);
            animHandler.InteractE2 = true;
            //yield return new WaitForSeconds(0.5f);
            inventory.AddItem(itemData, amount);
            Debug.Log($"Player mengambil iteem: {itemData.itemName} x{amount}");
            StartCoroutine(destroy());
        }
    }
    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.5f);
        EventCallBack.Vignette();
        Inirumah.SetActive(true);
        Destroy(gameObject);
    }

}
