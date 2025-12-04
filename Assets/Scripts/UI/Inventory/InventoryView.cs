using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public event Action<int> OnDescriptionRequested, OnStartDragging, OnClicked, OnDropRequested;

    public event Action<int, int> OnSwapItems, OnMultipleItemDrop;

    public event Action OnSortRequested;

    [SerializeField] 
    private ItemController _itemPrefab;

    [SerializeField] 
    private Tooltip _tooltip;

    [SerializeField] 
    private PointerFollower _pointerFollower;

    [SerializeField]
    private ItemDropView _itemDropView;

    [SerializeField]
    private RectTransform _contentPanel;

    [SerializeField]
    private Button _dropItemButton;

    [SerializeField]
    private Button _confirmDropButton;

    [SerializeField]
    private Button _sortButton;

    [SerializeField]
    private ItemController[] _equipment;

    private List<IItemView> _itemList = new();

    private int _currentDraggedItemIndex = -1;

    private int _selectedItemIndex = -1;

    private void Start()
    {
        _dropItemButton.onClick.AddListener(
            delegate { 
                if (_selectedItemIndex >= 0)
                    OnDropRequested?.Invoke(_selectedItemIndex); 
            });

        _confirmDropButton.onClick.AddListener(
            delegate {
                OnMultipleItemDrop?.Invoke(_selectedItemIndex, _itemDropView.SliderValue);
                _itemDropView.Disable();
            });

        _sortButton.onClick.AddListener(
            delegate {
                OnSortRequested?.Invoke();
            });
    }

    public void InitInventory(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            IItemController item = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _contentPanel);
            _itemList.Add(item as IItemView);
            SubscribeToItemController(item);
        }
    }

    public void InitEquipment(Dictionary<int, ItemTypeSO> equipment)
    {
        for (int i = 0; i < _equipment.Length; i++)
        {
            var equipmentItem = _equipment[i].GetComponent<EquipmentItemView>();
            equipmentItem.SetBackground(equipment[_itemList.Count].BackgroundSprite);
            _itemList.Add(_equipment[i]);
            SubscribeToItemController(_equipment[i]);
        }
    }

    public void SetDropWindow(int stackSize)
    {
        _itemDropView.SetSlider(stackSize);
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
        _selectedItemIndex = index;
    }

    public void ResetAllItems()
    {
        _itemList.ForEach(item => item.ResetData());
    }

    private void SubscribeToItemController(IItemController item)
    {
        item.OnItemBeginDrag += HandleBeginDrag;
        item.OnItemEndDrag += HandleItemEndDrag;
        item.OnItemDropped += HandleSwap;
        item.OnItemClicked += HandleItemClicked;
        item.OnPointerEntered += HandlePointerEntered;
        item.OnPointerExited += HandlePointerExited;
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
