using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    public event Action<int> OnDescriptionRequested, OnStartDragging, OnItemSelected;

    public event Action<int, int> OnSwapItems;

    [SerializeField] 
    private ItemPresenter _itemPrefab;

    [SerializeField] 
    private RectTransform _contentPanel;

    [SerializeField] 
    private TooltipPresenter _tooltip;

    [SerializeField] 
    private PointerFollower _pointerFollower;

    private List<ItemPresenter> _itemList = new();

    private int _currentDraggedItemIndex = -1; 

    private void Awake()
    {
        _tooltip.ResetTooltipData();
        _pointerFollower.Toggle(false);
    }

    public void InitInventory(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            ItemPresenter item = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(_contentPanel);
            item.transform.localScale = Vector3.one;
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
        {
            _itemList[index].SetData(sprite, count);
        }
    }

    public void CreateDraggedItem(Sprite sprite)
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
        {
            item.ResetData();
            item.SelectItem(false);
        }
    }

    private void ResetDraggedItem()
    {
        _pointerFollower.Toggle(false);
        _currentDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(ItemPresenter item)
    {
        int index = _itemList.IndexOf(item);
        if (index < 0) return;
        _currentDraggedItemIndex = index;
        HandleItemClicked(item);
        OnStartDragging?.Invoke(index);
    }

    private void HandleItemEndDrag(ItemPresenter item)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(ItemPresenter item)
    {
        int index = _itemList.IndexOf(item);
        if (index < 0) return;
        OnSwapItems?.Invoke(_currentDraggedItemIndex, index);
        HandleItemClicked(item);
    }

    private void HandleItemClicked(ItemPresenter item)
    {
        int index = _itemList.IndexOf(item);
        if (index < 0) return;
        OnItemSelected?.Invoke(index);
    }

    private void HandlePointerEntered(ItemPresenter item)
    {
        int index = _itemList.IndexOf(item);
        if (index < 0) return;
        OnDescriptionRequested?.Invoke(index);
    }

    private void HandlePointerExited(ItemPresenter item)
    {
        _tooltip.ResetTooltipData();
    }
}
