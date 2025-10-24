using System;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    private InventoryManager _inventoryManager;

    private int _id;

    public int Id { get => _id; set => _id = value; }

    public event Action OnSelect, OnDeselect;

    private void Awake()
    {
        _inventoryManager = InventoryManager.Instance;
    }

    public void Select() => OnSelect?.Invoke(); 

    public void Deselect() => OnDeselect?.Invoke();

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventoryManager.SelectSlot(_id);
        Select();
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (transform.childCount == 1) droppedItem.SetParent(transform);
        else if (droppedItem.ItemSO.Stackable) 
            CombineStackableItem(droppedItem, transform.GetChild(1).GetComponent<InventoryItem>());
    }

    private void CombineStackableItem(InventoryItem droppedItem, InventoryItem slotItem)
    {
        if (droppedItem.ItemSO.ItemType == slotItem.ItemSO.ItemType)
        {
            slotItem.Count += droppedItem.Count;
            droppedItem.Count = slotItem.Count - 5;
            if (slotItem.Count > 5) slotItem.Count = 5;
        }
    }
}
