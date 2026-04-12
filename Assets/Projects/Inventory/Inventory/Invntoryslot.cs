using UnityEngine;
[System.Serializable]
public class InventorySlot 
{
    public Item Item;
    public int X;
    public int Y;

    public InventorySlot(Item _item, int _x, int _y)
    {
        Item = _item;
        X = _x;
        Y = _y;
    }
}
