using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private ItemType _itemType;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        InventoryItem slotItem = transform.GetComponentInChildren<InventoryItem>();

        if (droppedItem.ItemSO.ItemType == _itemType)
        {
            if (slotItem == null) 
                droppedItem.SetParent(transform);
            else
            {
                var parent = droppedItem.Parent;
                droppedItem.SetParent(slotItem.Parent);
                slotItem.SetParent(parent);
            }
        }
    }
}
