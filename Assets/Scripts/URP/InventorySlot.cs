[System.Serializable]
public class InventorySlot
{
    public InventoryItem item;
    public int quantity;

    public InventorySlot(InventoryItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
