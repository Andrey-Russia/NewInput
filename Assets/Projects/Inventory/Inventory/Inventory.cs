using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action OnChanged;

    [SerializeField] private ItemDatabase database;
    [SerializeField] private List<Item> startItems;
    [SerializeField] internal int maxItems = 9;

    public List<InventorySlot> slots = new List<InventorySlot>();

    private string path;

    private void Awake()
    {
        path = Path.Combine(Application.persistentDataPath, "inv.json");
        database.Init();

        Load();

        if (slots.Count == 0)
        {
            foreach (var item in startItems)
                Add(item);
        }

        OnChanged?.Invoke();
    }

    public void Add(Item item)
    {
        if (item == null) return;

        if (slots.Count >= maxItems)
        {
            Debug.Log("Inventory is full");
            return;
        }

        var pos = FindFree();
        slots.Add(new InventorySlot(item, pos.x, pos.y));

        Save();
        OnChanged?.Invoke();
    }

    public void Remove(InventorySlot slot)
    {
        slots.Remove(slot);
        Save();
        OnChanged?.Invoke();
    }

    public void Clear()
    {
        slots.Clear();
        Save();
        OnChanged?.Invoke();
    }

    public Vector2Int FindFree()
    {
        for (int y = 0; y < 10; y++)
            for (int x = 0; x < 10; x++)
                if (!slots.Exists(s => s.X == x && s.Y == y))
                    return new Vector2Int(x, y);

        return Vector2Int.zero;
    }

    [Serializable]
    class SaveData
    {
        public List<SlotData> slots;

        [Serializable]
        public class SlotData
        {
            public string id;
            public int x, y;
        }
    }

    void Save()
    {
        var data = new SaveData();
        data.slots = new List<SaveData.SlotData>();

        foreach (var s in slots)
        {
            data.slots.Add(new SaveData.SlotData
            {
                id = s.Item.ID,
                x = s.X,
                y = s.Y
            });
        }

        File.WriteAllText(path, JsonUtility.ToJson(data, true));
    }

    void Load()
    {
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveData>(json);

        slots.Clear();

        foreach (var s in data.slots)
        {
            var item = database.Get(s.id);
            if (item != null)
                slots.Add(new InventorySlot(item, s.x, s.y));
        }
    }

    public Item GetRandomItem()
    {
        if (database.items == null || database.items.Count == 0)
        {
            Debug.LogError("Database empty");
            return null;
        }

        int index = UnityEngine.Random.Range(0, database.items.Count);
        return database.items[index];
    }
}