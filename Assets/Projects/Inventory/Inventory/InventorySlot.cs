[System.Serializable]
public class InventorySlot
{
    public Item Item;
    public int X;
    public int Y;

    public InventorySlot(Item item, int x, int y)
    {
        Item = item;
        X = x;
        Y = y;
    }
}