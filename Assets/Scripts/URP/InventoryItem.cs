using UnityEngine;

public enum ItemType
{
    Tool,
    KeyItem,
    CanEnterHouse,
    Consumable,
    LightSource,
    QuestItem
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isStackable = true;
    public ItemType itemType;
}
