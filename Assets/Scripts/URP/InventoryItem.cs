using UnityEngine;

public enum ItemType
{
    Tool,
    KeyItem,
    CanEnterHouse,
    Consumable,
    LightSource,   // untuk item lampu
    QuestItem,
    Boneka,
    Potion,
    Fragment,
    WallJump
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isStackable = true;
    public ItemType itemType;

    [Header("Light Settings (hanya dipakai kalau tipe = LightSource)")]
    public float itemIntensity = 1f;
    public float itemInnerRadius = 3f;
    public float itemOuterRadius = 18f;
    public float itemFalloff = 0.5f;

}
