using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemInventory> items = new List<ItemInventory>();
    [SerializeField] private DataBase data;

    [SerializeField] private GameObject Show;
    [SerializeField] private GameObject InventoryMainObject;

    [SerializeField] private int maxCount;
    [SerializeField] Camera cam;

    [SerializeField] internal EventSystem es;
    [SerializeField] internal int currentID;

    [SerializeField] internal ItemInventory currentItem;
    [SerializeField] internal RectTransform movinObject;

    [SerializeField] internal Vector3 offSet;

    private void Start()
    {    
        if (items.Count == 0)
        {
            AddGraphic();
        }

        for(int i = 0; i < maxCount; i++) //Тесе инвенторя. рандом заполнение ячеек
        {
            AddItem(i, data.items[Random.Range(0, data.items.Count)], Random.Range(1, 99));
        }
        UpdateInventory();
    }

    private void Update()
    {
        if (currentID != -1)
        {
            MoveObject();
        }
    }        

    internal void AddItem(int id, Item item, int count)
    {
        items[id].id = item.id;
        items[id].count = count;
        items[id].item.GetComponent<Image>().sprite = item.img;

        if (count < 1 && item.id != 0)
        {
            items[id].item.GetComponentInChildren<Text>().text = count.ToString();
        }
        else
        {
            items[id].item.GetComponentInChildren<Text>().text = "";
        }
    }

    internal void AddInventoryItem(int id, ItemInventory invitem)
    {
        items[id].id = invitem.id;
        items[id].count = invitem.count;
        items[id].item.GetComponent<Image>().sprite = data.items[invitem.id].img;

        if (invitem.count < 1 && invitem.id != 0)
        {
            items[id].item.GetComponentInChildren<Text>().text = invitem.count.ToString();
        }
        else
        {
            items[id].item.GetComponentInChildren<Text>().text = "";
        }
    }

    internal void AddGraphic()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newItem = Instantiate(Show, InventoryMainObject.transform) as GameObject;

            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.item = newItem;

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            Button tempButton = newItem.GetComponent<Button>();

            tempButton.onClick.AddListener(delegate { SelectObject(); });
            items.Add(ii);
        }
    }

    internal void SelectObject()
    {
        if (currentID == -1)
        {
            currentID = int.Parse(es.currentSelectedGameObject.name);
            currentItem = CopyInventoryItem(items[currentID]);
            movinObject.gameObject.SetActive(true);
            movinObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;

            AddItem(currentID, data.items[0], 0);
        }
        else
        {
            AddInventoryItem(currentID, items[int.Parse(es.currentSelectedGameObject.name)]);

            AddInventoryItem(int.Parse(es.currentSelectedGameObject.name), currentItem);
            currentID = -1;

            movinObject.gameObject.SetActive(false);
        }
    }

    internal void UpdateInventory()
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id != 0 && items[i].count > 1)
            {
                items[i].item.GetComponentInChildren<Text>().text = items[i].count.ToString(); 
            }
            else
            {
                items[i].item.GetComponentInChildren<Text>().text = "";
            }

            items[i].item.GetComponentInChildren<Image>().sprite = data.items[items[i].id].img;
        }
    }

    internal void MoveObject()
    {
        Vector3 pos = Input.mousePosition + offSet;
        pos.z = InventoryMainObject.GetComponent<RectTransform>().position.z;
        movinObject.position = cam.ScreenToViewportPoint(pos);
    }

    internal ItemInventory CopyInventoryItem(ItemInventory old)
    {
        ItemInventory New = new ItemInventory();
        New.id = old.id;
        New.item = old.item;
        New.count = old.count;

        return New;
    }
}

[System.Serializable]

internal class ItemInventory
{
    public int id;
    public GameObject item;
    public int count;
}