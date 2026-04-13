using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] Inventory inv;
    [SerializeField] Transform grid;
    [SerializeField] GameObject prefab;

    [SerializeField] Image icon;
    [SerializeField] TMP_Text nameText;

    [SerializeField] Button clearBtn;
    [SerializeField] Button chestBtn;

    void Start()
    {
        inv.OnChanged += Draw;
        clearBtn.onClick.AddListener(inv.Clear);
        chestBtn.onClick.AddListener(OpenChest);

        Draw();
    }

    void Draw()
    {
        foreach (Transform c in grid)
            Destroy(c.gameObject);

        foreach (var s in inv.slots)
        {
            if (s == null || s.Item == null)
            {
                Debug.LogError("Slot or Item is NULL");
                continue;
            }

            var go = Instantiate(prefab, grid);

            var img = go.GetComponent<Image>();

            if (img == null)
            {
                Debug.LogError("No Image on prefab");
                continue;
            }

            img.sprite = s.Item.Icon;

            var drag = go.AddComponent<DragItem>();
            drag.slot = s;
            drag.window = this;

            InventorySlot capturedSlot = s;

            var trigger = go.AddComponent<EventTrigger>();

            var leftClick = new EventTrigger.Entry();
            leftClick.eventID = EventTriggerType.PointerClick;
            leftClick.callback.AddListener((data) =>
            {
                var ev = (PointerEventData)data;

                if (ev.button == PointerEventData.InputButton.Left)
                {
                    Select(capturedSlot);
                }
            });
            trigger.triggers.Add(leftClick);

            var rightClick = new EventTrigger.Entry();
            rightClick.eventID = EventTriggerType.PointerClick;
            rightClick.callback.AddListener((data) =>
            {
                var ev = (PointerEventData)data;

                if (ev.button == PointerEventData.InputButton.Right)
                {
                    DeleteItem(capturedSlot);
                }
            });
            trigger.triggers.Add(rightClick);
        }

        chestBtn.interactable = inv.slots.Count < inv.maxItems;
    }

    void Select(InventorySlot s)
    {
        icon.sprite = s.Item.Icon;
        nameText.text = s.Item.Name + "\nÖåíà: " + s.Item.Price;
    }

    void DeleteItem(InventorySlot slot)
    {
        if (slot == null) return;

        inv.Remove(slot);
    }

    void OpenChest()
    {
        if (inv.slots.Count >= inv.maxItems)
        {
            Debug.Log("Inventory full");
            return;
        }

        Item item = inv.GetRandomItem();

        if (item != null)
            inv.Add(item);
    }
}