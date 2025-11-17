using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public List<InventoryItemData> _initialItems = new();

    [SerializeField] 
    private InventoryPresenter _inventory;

    [SerializeField]
    private InventorySO _inventoryData;

    private void Start()
    {
        InitUI();
        InitInventoryData();
    }

    private void InitInventoryData()
    {
        _inventoryData.Init();
        foreach (var item in _initialItems)
        {
            if (item.IsEmpty) continue;
            _inventoryData.AddItem(item.Item, item.Count);
        }
        UpdateInventoryUI();
    }

    private void InitUI()
    {
        _inventory.InitInventory(_inventoryData.Size);
        _inventory.OnDescriptionRequested += HandleDescriptionRequested;
        _inventory.OnStartDragging += HandleDragging;
        _inventory.OnSwapItems += HandleSwapItems;
        _inventory.OnItemSelected += HandleItemSelection;
    }

    private void UpdateInventoryUI()
    {
        _inventory.ResetAllItems();
        var inventoryState = _inventoryData.GetInventoryState();
        foreach (var item in inventoryState)
            _inventory.UpdateItemData(item.Key, item.Value.Item.Sprite, item.Value.Count);
    }

    private void HandleDescriptionRequested(int index)
    {
        InventoryItemData inventoryItem = _inventoryData.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;

        ItemSO item = inventoryItem.Item;
        _inventory.UpdateTooltip(item.Description);
    }

    private void HandleDragging(int index)
    {
        InventoryItemData inventoryItem = _inventoryData.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;
        _inventory.CreateDraggedItem(inventoryItem.Item.Sprite);
    }

    private void HandleSwapItems(int index_1, int index_2)
    {
        _inventoryData.SwapItems(index_1, index_2);
        UpdateInventoryUI();
    }

    private void HandleItemSelection(int index)
    {
        InventoryItemData inventoryItem = _inventoryData.GetItemAt(index);
        if (inventoryItem.IsEmpty)
        {
            _inventory.DeselectAllItems();
            return;
        }
        _inventory.SelectItem(index);
    }
}
