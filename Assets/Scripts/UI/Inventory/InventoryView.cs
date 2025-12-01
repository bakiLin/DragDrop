using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public event Action<int> OnDescriptionRequested, OnStartDragging, OnClicked;

    public event Action<int, int> OnSwapItems;

    [SerializeField] 
    private ItemController _itemPrefab;

    [SerializeField] 
    private Tooltip _tooltip;

    [SerializeField] 
    private PointerFollower _pointerFollower;

    [SerializeField]
    private RectTransform _contentPanel;

    private List<IItemView> _itemList = new();

    private int _currentDraggedItemIndex = -1;

    public void InitInventory(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            IItemController item = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _contentPanel);
            _itemList.Add(item as IItemView);

            item.OnItemBeginDrag += HandleBeginDrag;
            item.OnItemEndDrag += HandleItemEndDrag;
            item.OnItemDropped += HandleSwap;
            item.OnItemClicked += HandleItemClicked;
            item.OnPointerEntered += HandlePointerEntered;
            item.OnPointerExited += HandlePointerExited;
        }
    }

    public void UpdateItemData(int index, Sprite sprite, int count)
    {
        if (_itemList.Count > index)
            _itemList[index].SetData(sprite, count);
    }

    public void SetPointerFollower(Sprite sprite)
    {
        _pointerFollower.SetData(sprite);
        _pointerFollower.Toggle(true);
    }

    public void UpdateTooltip(string description)
    {
        _tooltip.SetTooltipData(description);
    }

    public void DeselectAllItems()
    {
        _itemList.ForEach(item => item.ToggleItem(false));
    }

    public void SelectItem(int index)
    {
        DeselectAllItems();
        _itemList[index].ToggleItem(true);
    }

    public void ResetAllItems()
    {
        _itemList.ForEach(item => item.ResetData());
    }

    private void HandleBeginDrag(IItemView item)
    {
        int index = _itemList.IndexOf(item);
        _currentDraggedItemIndex = index;
        OnStartDragging?.Invoke(index);
    }

    private void HandleItemEndDrag(IItemView item)
    {
        _pointerFollower.Toggle(false);
        _currentDraggedItemIndex = -1;
    }

    private void HandleSwap(IItemView item)
    {
        int index = _itemList.IndexOf(item);
        if (index < 0 || _currentDraggedItemIndex < 0) return;
        OnSwapItems?.Invoke(_currentDraggedItemIndex, index);
    }

    private void HandleItemClicked(IItemView item)
    {
        int index = _itemList.IndexOf(item);
        SelectItem(index);
        OnClicked?.Invoke(index);
    }

    private void HandlePointerEntered(IItemView item)
    {
        int index = _itemList.IndexOf(item);
        OnDescriptionRequested?.Invoke(index);
    }

    private void HandlePointerExited(IItemView item)
    {
        _tooltip.ResetTooltipData();
    }
}
