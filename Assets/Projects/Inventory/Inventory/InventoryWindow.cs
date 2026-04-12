using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

internal class InventoryWindow : MonoBehaviour
{
    [SerializeField] Inventory targetInventory; [SerializeField] RectTransform itemsPanel;

    [Header("UI Элементы")]
    [SerializeField] private GameObject itemIconPrefab;

    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private TextMeshProUGUI selectedItemName;

    [SerializeField] private Button existingChestButton;
    [SerializeField] private Button existingClearButton;

    List<GameObject> itemsIcons = new List<GameObject>();

    private const int MaxInventorySize = 10;

    private void Start()
    {
        targetInventory.OnItemAdded += OnItemAdded;
        targetInventory.OnItemRemoved += HandleItemRemoved;
        targetInventory.OnInventoryCleared += Redraw;

        existingChestButton.onClick.AddListener(OpenChest);
        existingClearButton.onClick.AddListener(ClearInventory);

        DontDestroyOnLoad(existingChestButton?.gameObject);
        DontDestroyOnLoad(existingClearButton?.gameObject);

        Redraw();
    }

    private void OnDestroy()
    {
        targetInventory.OnItemAdded -= OnItemAdded;
        targetInventory.OnItemRemoved -= HandleItemRemoved;
        targetInventory.OnInventoryCleared -= Redraw;
    }

    private void OnItemAdded(Item addedItem)
    {
        if (targetInventory.inventorySlots.Count >= MaxInventorySize)
        {
            return;
        }

        Redraw();
    }

    private void HandleItemRemoved(Item removedItem)
    {
        var slot = targetInventory.inventorySlots.FirstOrDefault(s => s.Item == removedItem);

        if (slot != null)
        {
            OnItemRemoved(slot.Item);
        }
    }

    private void OnItemRemoved(Item removedItem) => Redraw();

    public void Redraw()
    {
        ClearDrawn();

        foreach (var slot in targetInventory.inventorySlots.Take(MaxInventorySize))
        {
            CreateIcon(slot);
        }
    }

    private void CreateIcon(InventorySlot slot)
    {
        GameObject iconObj = Instantiate(itemIconPrefab, itemsPanel);
        Image iconImage = iconObj.GetComponent<Image>();
        Button iconButton = iconObj.GetComponent<Button>();

        iconImage.sprite = slot.Item.Icon;

        RectTransform rectTransform = iconObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(slot.X * 50, -slot.Y * 50);

        EventTrigger trigger = iconButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry leftClickEntry = new EventTrigger.Entry();
        leftClickEntry.eventID = EventTriggerType.PointerClick;
        leftClickEntry.callback.AddListener((data) =>
        {
            PointerEventData ped = data as PointerEventData;
            if (ped.button == PointerEventData.InputButton.Left)
            {
                ShowSelectedInfo(slot);
            }
        });
        trigger.triggers.Add(leftClickEntry);

        EventTrigger.Entry rightClickEntry = new EventTrigger.Entry();
        rightClickEntry.eventID = EventTriggerType.PointerClick;
        rightClickEntry.callback.AddListener((data) =>
        {
            PointerEventData ped = data as PointerEventData;
            if (ped.button == PointerEventData.InputButton.Right)
            {
                DeleteItem(slot);
            }
        });
        trigger.triggers.Add(rightClickEntry);

        itemsIcons.Add(iconObj);
    }

    void ClearDrawn()
    {
        foreach (var obj in itemsIcons)
        {
            Destroy(obj);
        }
        itemsIcons.Clear();
    }

    void ShowSelectedInfo(InventorySlot slot)
    {
        selectedItemIcon.sprite = slot.Item.Icon;
        selectedItemIcon.enabled = true;

        selectedItemName.text = $"{slot.Item.Name}\nЦена: {slot.Item.Price}";
    }

    void OpenChest()
    {
        if (targetInventory.StartItems.Count > 0)
        {
            int randomIndex = Random.Range(0, targetInventory.StartItems.Count);
            targetInventory.AddItem(targetInventory.StartItems[randomIndex]);
        }
    }

    private void DeleteItem(InventorySlot slot)
    {
        if (slot != null)
        {
            targetInventory.RemoveItem(slot.Item);
        }
    }

    private void ClearInventory()
    {
        targetInventory.ClearInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}