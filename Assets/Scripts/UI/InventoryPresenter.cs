using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    public event Action<int> OnDescriptionRequested, OnStartDragging, OnDoubleClicked;

    public event Action<int, int> OnSwapItems;

    [SerializeField] 
    private ItemPresenter _itemPrefab;

    [SerializeField] 
    private Tooltip _tooltip;

    [SerializeField] 
    private PointerFollower _pointerFollower;

    [SerializeField]
    private RectTransform _contentPanel;

    [SerializeField]
    private float _doubleClickTime;

    private List<ItemPresenter> _itemList = new();

    private int _currentDraggedItemIndex = -1;

    private float _lastTimeClicked;

    public void InitInventory(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            var item = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _contentPanel);
            _itemList.Add(item);

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
        foreach (var item in _itemList)
            item.SelectItem(false);
    }

    public void SelectItem(int index)
    {
        DeselectAllItems();
        _itemList[index].SelectItem(true);
    }

    public void ResetAllItems()
    {
        foreach (var item in _itemList)
            item.ResetData();
    }

    private void HandleBeginDrag(ItemPresenter item)
    {
        int index = _itemList.IndexOf(item);
        _currentDraggedItemIndex = index;
        OnStartDragging?.Invoke(index);
    }

    private void HandleItemEndDrag(ItemPresenter item)
    {
        _pointerFollower.Toggle(false);
        _currentDraggedItemIndex = -1;
    }

    private void HandleSwap(ItemPresenter item)
    {
        int index = _itemList.IndexOf(item);
        if (index < 0 || _currentDraggedItemIndex < 0) return;
        OnSwapItems?.Invoke(_currentDraggedItemIndex, index);
    }

    private void HandleItemClicked(ItemPresenter item)
    {
        int index = _itemList.IndexOf(item);
        SelectItem(index);

        float clickTimeDelta = Time.unscaledTime - _lastTimeClicked;
        if (clickTimeDelta < _doubleClickTime && clickTimeDelta > 0.1f)
        {
            _lastTimeClicked = 0f;
            OnDoubleClicked?.Invoke(index);
            return;
        }
        _lastTimeClicked = Time.unscaledTime;
    }

    private void HandlePointerEntered(ItemPresenter item)
    {
        int index = _itemList.IndexOf(item);
        OnDescriptionRequested?.Invoke(index);
    }

    private void HandlePointerExited(ItemPresenter item)
    {
        _tooltip.ResetTooltipData();
    }
}
