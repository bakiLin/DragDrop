using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryManager : MonoBehaviour
{
    [Inject] private DiContainer _container;

    [Inject] private ItemDrop _itemDrop;

    [SerializeField] private int _maxStack;

    [SerializeField] private float _doubleClickTime;

    [SerializeField] private GameObject _inventoryItemPrefab;

    [SerializeField] private DropdownManager _dropdownManager;

    [SerializeField] private ItemSO[] _sortOrder;

    [SerializeField] private InventorySlot[] _inventorySlots;

    [SerializeField] private Equipment[] _equipment;

    private int _selectedSlot = -1;

    private float _lastClickTime;

    private void Start()
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
            _inventorySlots[i].Id = i;
    }

    public void AddItem()
    {
        ItemSO item = _dropdownManager.GetSelectedItem();

        if (item != null)
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                var slotItem = _inventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (slotItem != null && item.Stackable && slotItem.ItemSO == item && slotItem.Count < _maxStack)
                {
                    slotItem.Count++;
                    return;
                }
            }

            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                var slot = _inventorySlots[i];
                if (!slot.GetComponentInChildren<InventoryItem>())
                {
                    var itemObj = _container.InstantiatePrefab(_inventoryItemPrefab, slot.transform).GetComponent<InventoryItem>();
                    itemObj.transform.SetAsFirstSibling();
                    itemObj.Initialize(item);
                    return;
                }
            }
        }
    }

    public void DropItem()
    {
        if (_selectedSlot >= 0)
        {
            var slotItem = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
            if (slotItem != null)
            {
                if (slotItem.Count == 1) Destroy(slotItem.gameObject);
                else _itemDrop.InitializeSlider(slotItem.Count);
            }
        }
    }

    public void ConfirmDrop()
    {
        var slotItem = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
        slotItem.Count -= _itemDrop.ConfirmItemDrop();
    }

    public void SelectSlot(int id)
    {
        if (_selectedSlot >= 0) _inventorySlots[_selectedSlot].Deselect();
        if (id == _selectedSlot && Time.time - _lastClickTime < _doubleClickTime) DoubleClick();

        _selectedSlot = id;
        _lastClickTime = Time.time;
    }

    private void DoubleClick()
    {
        var clickedItem = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
        if (clickedItem == null) return;
        else
        {
            for (int i = 0; i < _equipment.Length; i++)
            {
                if (_equipment[i].ItemType == clickedItem.ItemSO.ItemType)
                {
                    var slotItem = _equipment[i].Slot.GetComponentInChildren<InventoryItem>();
                    if (slotItem == null)
                        clickedItem.SetParent(_equipment[i].Slot);
                    else
                    {
                        var parent = clickedItem.Parent;
                        clickedItem.SetParent(slotItem.Parent);
                        slotItem.SetParent(parent);
                    }
                    return;
                }
            }
        }
    }

    public void Sort()
    {
        List<InventoryItem> itemList = new List<InventoryItem>();
        int orderCounter = 0, itemCounter = 0;

        while (orderCounter < _sortOrder.Length)
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventoryItem child = _inventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (child != null && child.ItemSO == _sortOrder[orderCounter])
                {
                    itemList.Add(child);
                    itemCounter++;
                }
            }
            orderCounter++;
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].SetParent(_inventorySlots[i].transform);
            itemList[i].transform.localPosition = Vector3.zero;
        }
    }
}
