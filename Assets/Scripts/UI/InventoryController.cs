using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] 
    private InventoryPresenter _inventoryUI;

    [SerializeField]
    private InventorySO _inventoryData;

    private void Start()
    {
        _inventoryData.OnInventoryDataChanged += UpdateInventoryUI;

        InitUI();
        InitInventoryData();
    }

    private void InitInventoryData()
    {
        _inventoryData.Init();
    }

    private void InitUI()
    {
        _inventoryUI.InitInventory(_inventoryData.Size);
        _inventoryUI.OnDescriptionRequested += HandleDescriptionRequested;
        _inventoryUI.OnStartDragging += HandleDragging;
        _inventoryUI.OnSwapItems += HandleSwapItems;
        _inventoryUI.OnItemSelected += HandleItemSelection;
        _inventoryUI.OnDoubleClicked += HandleDoubleClicking;
    }

    private void HandleDoubleClicking(int index)
    {
        InventoryItemData inventoryItem = _inventoryData.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;

        IDestroyable item = inventoryItem.Item as IDestroyable;
        if (item != null) _inventoryData.RemoveItem(index, 1);
    }

    private void UpdateInventoryUI()
    {
        _inventoryUI.ResetAllItems();
        var inventoryState = _inventoryData.GetInventoryState();
        foreach (var item in inventoryState)
            _inventoryUI.UpdateItemData(item.Key, item.Value.Item.Sprite, item.Value.Count);
    }

    private void HandleDescriptionRequested(int index)
    {
        InventoryItemData inventoryItem = _inventoryData.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;

        ItemSO item = inventoryItem.Item;
        _inventoryUI.UpdateTooltip(item.Description);
    }

    private void HandleDragging(int index)
    {
        InventoryItemData inventoryItem = _inventoryData.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;
        _inventoryUI.CreateDraggedItem(inventoryItem.Item.Sprite);
    }

    private void HandleSwapItems(int index_1, int index_2)
    {
        _inventoryData.SwapItems(index_1, index_2);
    }

    private void HandleItemSelection(int index)
    {
        InventoryItemData inventoryItem = _inventoryData.GetItemAt(index);
        if (inventoryItem.IsEmpty)
        {
            _inventoryUI.DeselectAllItems();
            return;
        }
        _inventoryUI.SelectItem(index);
    }
}
