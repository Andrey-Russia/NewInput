using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot slot;
    public InventoryWindow window;

    private RectTransform rect;
    private CanvasGroup group;
    private Vector3 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        group = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rect.position;
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rect.position = startPos;
        group.blocksRaycasts = true;
    }
}