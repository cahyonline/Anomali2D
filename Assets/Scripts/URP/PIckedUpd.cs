using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItem itemData; 
    public int amount = 1;         

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("dsfj");
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null && itemData != null)
            {
                inventory.AddItem(itemData, amount);
                Debug.Log($"Player mengambil item: {itemData.itemName} x{amount}");
                Destroy(gameObject);
            }
        }
    }
}
