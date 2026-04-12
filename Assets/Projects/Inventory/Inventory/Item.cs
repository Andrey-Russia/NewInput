using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemId;
    public string Name = "Item";
    public Sprite Icon;
    public int Price = 500;

    public string ID => itemId;
}
