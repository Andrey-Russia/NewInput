using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action<Item> OnItemAdded;
    public Action<Item> OnItemRemoved;
    public Action OnInventoryCleared;

    [SerializeField] internal List<Item> StartItems = new List<Item>();
    [SerializeField] private int gridWidth = 5;

    internal List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "inventory_save.json");
        LoadInventory();

        if (inventorySlots.Count == 0)
        {
            for (int i = 0; i < StartItems.Count; i++)
            {
                AddItem(StartItems[i]);
            }
        }
    }

    private void LogJson(string json)
    {
        Debug.Log($"Saved JSON:\n{json}");
    }

    public void AddItem(Item item)
    {
        if (item == null) return;

        int x = 0, y = 0;
        FindEmptySlot(ref x, ref y);

        var newSlot = new InventorySlot(item, x, y);
        inventorySlots.Add(newSlot);

        OnItemAdded?.Invoke(newSlot.Item);
        SaveInventory();
    }

    public void RemoveItem(Item itemToRemove)
    {
        var slotToRemove = inventorySlots.Find(s => s.Item == itemToRemove);
        if (slotToRemove != null)
        {
            inventorySlots.Remove(slotToRemove);
            OnItemRemoved?.Invoke(slotToRemove.Item);
            SaveInventory();
        }
    }

    public void ClearInventory()
    {
        inventorySlots.Clear();
        OnInventoryCleared?.Invoke();
        SaveInventory();
    }

    [System.Serializable]
    private class SaveData
    {
        public List<SlotData> slots;

        [System.Serializable]
        public class SlotData
        {
            public string itemID;
            public int x, y;

            public SlotData(string id, int _x, int _y)
            {
                itemID = id;
                x = _x;
                y = _y;
            }
        }
    }

    private void SaveInventory()
    {
        var data = new SaveData();
        data.slots = new List<SaveData.SlotData>();

        foreach (var slot in inventorySlots)
        {
            data.slots.Add(new SaveData.SlotData(slot.Item.ID, slot.X, slot.Y));
        }

        string json = JsonUtility.ToJson(data);
        LogJson(json);
        File.WriteAllText(savePath, json);
    }

    private void LoadInventory()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        var data = JsonUtility.FromJson<SaveData>(json);

        inventorySlots.Clear();

        foreach (var slotData in data.slots)
        {
            Item foundItem = FindItemByID(slotData.itemID);
            if (foundItem != null)
            {
                inventorySlots.Add(new InventorySlot(foundItem, slotData.x, slotData.y));
                OnItemAdded?.Invoke(inventorySlots[inventorySlots.Count - 1].Item);
            }
        }
    }

    private void FindEmptySlot(ref int x, ref int y)
    {
        bool slotFound = false;
        for (int iY = 0; iY < 100; iY++)
        {
            for (int iX = 0; iX < gridWidth; iX++)
            {
                if (!IsSlotOccupied(iX, iY))
                {
                    x = iX;
                    y = iY;
                    slotFound = true;
                    break;
                }
            }
            if (slotFound) break;
        }
    }

    private bool IsSlotOccupied(int x, int y)
    {
        return inventorySlots.Exists(s => s.X == x && s.Y == y);
    }

    private Item FindItemByID(string id)
    {
        return null;
    }
}