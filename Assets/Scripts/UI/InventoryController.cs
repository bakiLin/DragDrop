using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] 
    private InventoryView _presenter;

    [SerializeField]
    private InventorySO _data;

    [SerializeField]
    private float _doubleClickTime;

    private float _lastClickedTime;

    private int _lastClickedIndex = -1;

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
        _presenter.OnClicked += HandleClicking;
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

        IEdible item = inventoryItem.Item as IEdible;
        if (item != null) _data.RemoveItem(index);
    }
}
