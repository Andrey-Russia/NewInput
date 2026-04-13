using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Database")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items;

    private Dictionary<string, Item> dict;

    public void Init()
    {
        dict = new Dictionary<string, Item>();

        foreach (var item in items)
        {
            if (!dict.ContainsKey(item.ID))
                dict.Add(item.ID, item);
        }
    }

    public Item Get(string id)
    {
        if (dict == null) Init();

        return dict.ContainsKey(id) ? dict[id] : null;
    }
}