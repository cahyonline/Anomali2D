using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // biar bisa pakai Light2D

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventory = new List<InventorySlot>();
    [SerializeField] private GameObject Kunci;
    [Header("Player Light")]
    [SerializeField] private Light2D playerLight;   // drag Light2D player ke sini di Inspector
    [SerializeField] private Light2D defaultLight;  // opsional, buat reset ke default

    public void AddItem(InventoryItem item, int amount = 1)
    {
        if (item.isStackable)
        {
            InventorySlot slot = inventory.Find(s => s.item == item);
            if (slot != null)
            {
                slot.quantity += amount;
                TriggerItemEvent(item);
                PrintInventory();
                HandleLightItem(item);
                return;
            }
        }

        inventory.Add(new InventorySlot(item, amount));
        TriggerItemEvent(item);
        PrintInventory();
        HandleLightItem(item);
    }

    public void RemoveItem(InventoryItem item, int amount = 1)
    {
        InventorySlot slot = inventory.Find(s => s.item == item);
        if (slot != null)
        {
            slot.quantity -= amount;
            if (slot.quantity <= 0)
            {
                inventory.Remove(slot);

                if (item.itemType == ItemType.KeyItem && Kunci != null)
                    Kunci.SetActive(false);

                if (item.itemType == ItemType.LightSource)
                    ResetLight(); // kalau lampu habis -> balik ke default
            }
            PrintInventory();
        }
    }

    private void TriggerItemEvent(InventoryItem item)
    {
        if (item.itemType == ItemType.KeyItem && Kunci != null)
        {
            Kunci.SetActive(true);
        }
    }

    private void PrintInventory()
    {
        string log = "📦 Inventory sekarang: ";
        if (inventory.Count == 0)
        {
            log += "Kosong";
        }
        else
        {
            foreach (var slot in inventory)
            {
                log += $"{slot.item.itemName} x{slot.quantity}, ";
            }
        }
        Debug.Log(log);
    }

    private void HandleLightItem(InventoryItem item)
    {
        if (item.itemType == ItemType.LightSource && playerLight != null)
        {
            playerLight.intensity = item.itemIntensity;
            playerLight.pointLightInnerRadius = item.itemInnerRadius;
            playerLight.pointLightOuterRadius = item.itemOuterRadius;
            playerLight.falloffIntensity = item.itemFalloff;

            Debug.Log($"💡 PlayerLight updated oleh item: {item.itemName}");
        }
    }

    private void ResetLight()
    {
        if (defaultLight != null && playerLight != null)
        {
            playerLight.intensity = defaultLight.intensity;
            playerLight.pointLightInnerRadius = defaultLight.pointLightInnerRadius;
            playerLight.pointLightOuterRadius = defaultLight.pointLightOuterRadius;
            playerLight.falloffIntensity = defaultLight.falloffIntensity;

            Debug.Log("💡 PlayerLight direset ke default.");
        }
    }

    public bool HasItem(string itemName) => inventory.Exists(s => s.item.itemName == itemName);
    public bool HasItem(ItemType itemType) => inventory.Exists(s => s.item.itemType == itemType);
    public InventorySlot GetItemByType(ItemType type) => inventory.Find(s => s.item.itemType == type);
    public InventorySlot GetItemByName(string itemName) => inventory.Find(s => s.item.itemName == itemName);
    public List<InventorySlot> GetAllItems() => inventory;
}
