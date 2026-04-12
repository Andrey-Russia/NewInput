using UnityEngine;

public class TakeItem : MonoBehaviour
{
    [SerializeField] Item itemToAdd;
    [SerializeField] Inventory targetInventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetInventory.AddItem(itemToAdd);
        }
    }
}
