using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<InventorySlot> inventory = new List<InventorySlot>();

    public void AddItem(InventoryItem item, int amount = 1)
    {
        if (item.isStackable)
        {
            InventorySlot slot = inventory.Find(s => s.item == item);
            if (slot != null)
            {
                slot.quantity += amount;
                return;
            }
        }

        inventory.Add(new InventorySlot(item, amount));
    }

    public bool HasItem(string itemName)
    {
        return inventory.Exists(s => s.item.itemName == itemName);
    }

    public bool HasItem(ItemType itemType)
    {
        return inventory.Exists(s => s.item.itemType == itemType);
    }

    public InventorySlot GetItemByType(ItemType type)
    {
        return inventory.Find(s => s.item.itemType == type);
    }

    public InventorySlot GetItemByName(string itemName)
    {
        return inventory.Find(s => s.item.itemName == itemName);
    }

    public List<InventorySlot> GetAllItems()
    {
        return inventory;
    }

    public void RemoveItem(InventoryItem item, int amount = 1)
    {
        InventorySlot slot = inventory.Find(s => s.item == item);
        if (slot != null)
        {
            slot.quantity -= amount;
            if (slot.quantity <= 0)
                inventory.Remove(slot);
        }
    }
}
