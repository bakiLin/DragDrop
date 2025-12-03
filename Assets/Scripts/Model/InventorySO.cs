using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/InventorySO", fileName = "InventorySO")]
public class InventorySO : ScriptableObject
{
    public event Action OnInventoryDataChanged;

    [field: SerializeField, Range(1, 20)]
    public int Size { get; private set; }

    public int EquipmentSize => _equippableItemTypes.Length;

    [SerializeField, Range(1, 100)]
    private int _maxStackSize;

    [SerializeField]
    private ItemTypeSO[] _equippableItemTypes;

    private List<InventoryItemData> _itemDataList;

    public void Init()
    {
        _itemDataList = Enumerable.Range(0, Size + EquipmentSize)
            .Select(_ => new InventoryItemData(_ >= Size))
            .ToList();
    }

    public Dictionary<int, ItemTypeSO> GetEquipmentType()
    {
        return Enumerable.Range(Size, EquipmentSize)
            .ToDictionary(i => i, i => _equippableItemTypes[i - Size]);    
    }

    public void AddItem(ItemSO item)
    {
        (item.IsStackable 
            ? (Action<ItemSO>)AddStackableItem : AddItemToFirstFreeSlot)(item);
        OnInventoryDataChanged?.Invoke();
    }

    public void RemoveItem(int index)
    {
        if (index >= _itemDataList.Count || _itemDataList[index].IsEmpty)
            return;

        var item = _itemDataList[index];
        _itemDataList[index] = item.Count > 1
            ? new InventoryItemData(item.Item, item.Count - 1)
            : new InventoryItemData();
        OnInventoryDataChanged?.Invoke();
    }

    public Dictionary<int, InventoryItemData> GetInventory()
    {
        return _itemDataList.Select((data, index) => (data, index))
            .Where(x => !x.data.IsEmpty)
            .ToDictionary(x => x.index, x => x.data);
    }

    public void SwapItems(int i1, int i2)
    {
        var (d1, d2) = (_itemDataList[i1], _itemDataList[i2]);

        if (d2.IsEquipment && _equippableItemTypes[i2 - Size].IsItemType(d1.Item))
        {
            (_itemDataList[i1], _itemDataList[i2]) = (
                new InventoryItemData(d2.Item, d2.Count),
                new InventoryItemData(d1.Item, d1.Count, true));
        }
        else if (d1.IsEquipment && (_itemDataList[i2].Item == null 
            || _equippableItemTypes[i1 - Size].IsItemType(d2.Item)))
        {
            (_itemDataList[i1], _itemDataList[i2]) = (
                new InventoryItemData(d2.Item, d2.Count, true),
                new InventoryItemData(d1.Item, d1.Count));
        }
        else if (!d1.IsEquipment && !d2.IsEquipment)
            (_itemDataList[i1], _itemDataList[i2]) = (d2, d1);

        OnInventoryDataChanged?.Invoke();
    }

    public void EquipItem(int index)
    {
        var item = _itemDataList[index].Item;
        int i = Array.FindIndex(_equippableItemTypes, _ => _.IsItemType(item));
        SwapItems(index, Size + i);
    }

    public InventoryItemData GetItemAt(int index)
    {
        return _itemDataList[index];
    }

    private void AddItemToFirstFreeSlot(ItemSO item)
    {
        int index = _itemDataList.FindIndex(x => x.IsEmpty);
        if (index >= 0) _itemDataList[index] = new InventoryItemData(item, 1);
    }

    private void AddStackableItem(ItemSO item)
    {
        int index = _itemDataList.FindIndex(
            x => !x.IsEmpty && x.Item.Id == item.Id && _maxStackSize - x.Count > 0);

        if (index >= 0)
            _itemDataList[index] = new InventoryItemData(_itemDataList[index].Item,
                _itemDataList[index].Count + 1);
        else
            AddItemToFirstFreeSlot(item);
    }
}