using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class InventoryController : MonoBehaviour
{
    [Inject] 
    private InventoryView _view;

    [Inject]
    private InventorySO _data;

    [SerializeField]
    private float _doubleClickTime;

    private float _lastClickedTime;

    private int _lastClickedIndex = -1;

    private void Start()
    {
        InitData();
        InitUI();
    }

    private void InitData()
    {
        _data.OnInventoryDataChanged += UpdateInventoryUI;
        _data.Init();
    }

    private void InitUI()
    {
        _view.InitInventory(_data.Size);
        _view.InitEquipment(_data.GetEquipmentType());
        _view.OnDescriptionRequested += HandleDescriptionRequested;
        _view.OnStartDragging += HandleDragging;
        _view.OnSwapItems += HandleSwapItems;
        _view.OnClicked += HandleClicking;
        _view.OnDropRequested += HandleItemDropping;
        _view.OnMultipleItemDrop += HandleMultipleItemDropping;
        _view.OnSortRequested += HandleSorting;
    }

    private void UpdateInventoryUI()
    {
        _view.ResetAllItems();
        foreach (var item in _data.GetInventory())
            _view.UpdateItemData(item.Key, item.Value.Item.Sprite, item.Value.Count);
    }

    private void HandleDescriptionRequested(int index)
    {
        InventoryItemData inventoryItem = _data.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;
        _view.UpdateTooltip(index, inventoryItem.Item.Description);
    }

    private void HandleDragging(int index)
    {
        InventoryItemData inventoryItem = _data.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;
        _view.SetPointerFollower(inventoryItem.Item.Sprite);
    }

    private void HandleSwapItems(int i1, int i2)
    {
        _data.SwapItems(i1, i2);
    }

    private void HandleClicking(int index)
    {
        var delta = Time.unscaledTime - _lastClickedTime;
        var isDoubleClick = delta < _doubleClickTime && delta > 0.1f && _lastClickedIndex == index;
        _lastClickedTime = isDoubleClick ? 0f : Time.unscaledTime; 
        if (isDoubleClick) HandleDoubleClicking(index);
        _lastClickedIndex = index;
    }

    private void HandleDoubleClicking(int index)
    {
        InventoryItemData inventoryItem = _data.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;

        ItemSO item = inventoryItem.Item;
        if (item != null) item.Interact(_data, index);
    }

    private void HandleItemDropping(int index)
    {
        var count = _data.GetItemAt(index).Count;
        if (count > 1) _view.SetDropWindow(count);
        else _data.RemoveItem(index);
    }

    private void HandleMultipleItemDropping(int index, int itemDropNum)
    {
        _data.RemoveItem(index, itemDropNum);
    }

    private void HandleSorting()
    {
        List<ItemSO> items = _data.ItemTypes
            .SelectMany(_ => _.Items)
            .ToList();

        List<InventoryItemData> newData = new();
        foreach (var item in items)
        {
            newData.AddRange(_data.GetInventory().Values
                .Where(_ => _.Item == item && !_.IsEquipment));
        }

        while (newData.Count < _data.Size)
            newData.Add(new InventoryItemData());

        _data.SetInventory(newData);
    }
}
