using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string ID;
    public string Name;
    public int Price;
    public Sprite Icon;
}