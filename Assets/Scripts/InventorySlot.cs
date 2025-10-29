using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Inject] private InventoryManager _inventoryManager;

    private int _id;

    public int Id { get => _id; set => _id = value; }

    public event Action OnSelect, OnDeselect;

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
        InventoryItem slotItem = transform.GetComponentInChildren<InventoryItem>();

        if (slotItem == null)
            droppedItem.SetParent(transform);
        else if (CombineStackableItem(droppedItem, slotItem))
            return;
        else
        {
            var parent = droppedItem.Parent;
            droppedItem.SetParent(slotItem.Parent);
            slotItem.SetParent(parent);
        }
    }

    private bool CombineStackableItem(InventoryItem droppedItem, InventoryItem slotItem)
    {
        if (droppedItem.ItemSO.Stackable) return false;
        if (droppedItem.ItemSO.name == slotItem.ItemSO.name)
        {
            slotItem.Count += droppedItem.Count;
            droppedItem.Count = slotItem.Count - 5;
            if (slotItem.Count > 5) slotItem.Count = 5;
            return true;
        }
        return false;
    }
}
