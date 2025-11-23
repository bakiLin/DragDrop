using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] 
    private InventoryPresenter _presenter;

    [SerializeField]
    private InventorySO _data;

    private void Start()
    {
        InitUI();
        _data.Init();
    }

    private void InitUI()
    {
        _data.OnInventoryDataChanged += UpdateInventoryUI;
        _presenter.InitInventory(_data.Size);
        _presenter.OnDescriptionRequested += HandleDescriptionRequested;
        _presenter.OnStartDragging += HandleDragging;
        _presenter.OnSwapItems += HandleSwapItems;
        _presenter.OnItemSelected += HandleItemSelection;
        _presenter.OnDoubleClicked += HandleDoubleClicking;
    }

    private void UpdateInventoryUI()
    {
        _presenter.ResetAllItems();
        foreach (var item in _data.GetInventory())
            _presenter.UpdateItemData(item.Key, item.Value.Item.Sprite, item.Value.Count);
    }

    private void HandleDescriptionRequested(int index)
    {
        InventoryItemData inventoryItem = _data.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;

        ItemSO item = inventoryItem.Item;
        _presenter.UpdateTooltip(item.Description);
    }

    private void HandleDragging(int index)
    {
        InventoryItemData inventoryItem = _data.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;
        _presenter.SetPointerFollower(inventoryItem.Item.Sprite);
    }

    private void HandleSwapItems(int index_1, int index_2)
    {
        _data.SwapItems(index_1, index_2);
    }

    private void HandleItemSelection(int index)
    {
        InventoryItemData inventoryItem = _data.GetItemAt(index);
        if (inventoryItem.IsEmpty)
        {
            _presenter.DeselectAllItems();
            return;
        }
        _presenter.SelectItem(index);
    }

    private void HandleDoubleClicking(int index)
    {
        InventoryItemData inventoryItem = _data.GetItemAt(index);
        if (inventoryItem.IsEmpty) return;

        IDestroyable item = inventoryItem.Item as IDestroyable;
        if (item != null) _data.RemoveItem(index);
    }
}
