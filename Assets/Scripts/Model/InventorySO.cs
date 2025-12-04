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

    [SerializeField, Range(1, 100)]
    private int _maxStackSize;

    [field: SerializeField]
    public ItemTypeSO[] ItemTypes { get; private set; }

    public int EquipmentSize => _equippableItemTypes.Length;

    private ItemTypeSO[] _equippableItemTypes;

    private List<InventoryItemData> _itemDataList;

    public void Init()
    {
        _equippableItemTypes = ItemTypes.Where(_ => _.IsEquippable).ToArray();

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

    public void RemoveItem(int index, int itemRemoveNumber = 1)
    {
        if (index >= _itemDataList.Count || _itemDataList[index].IsEmpty)
            return;

        var item = _itemDataList[index];
        _itemDataList[index] = item.Count - itemRemoveNumber > 0
            ? new InventoryItemData(item.Item, item.Count - itemRemoveNumber)
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
        var (data_1, data_2) = (_itemDataList[i1], _itemDataList[i2]);

        if (data_2.IsEquipment && _equippableItemTypes[i2 - Size].IsItemType(data_1.Item))
            SwapEquipment(i1, i2, data_1, data_2, false, true);
        else if (data_1.IsEquipment)
        {
            if (data_2.IsEquipment)
            {
                if (_equippableItemTypes[i1 - Size].IsItemType(data_2.Item)
                && _equippableItemTypes[i2 - Size].IsItemType(data_1.Item))
                    SwapEquipment(i1, i2, data_1, data_2, true, true);
            }
            else if (_itemDataList[i2].Item == null
                || _equippableItemTypes[i1 - Size].IsItemType(data_2.Item))
                SwapEquipment(i1, i2, data_1, data_2, true, false);
        }
        else if (!data_1.IsEquipment && !data_2.IsEquipment)
            (_itemDataList[i1], _itemDataList[i2]) = (data_2, data_1);

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

    private void SwapEquipment(int i1, int i2, InventoryItemData d1, InventoryItemData d2, 
        bool isEquipment_1, bool isEquipment_2)
    {
        _itemDataList[i1] = new InventoryItemData(d2.Item, d2.Count, isEquipment_1);
        _itemDataList[i2] = new InventoryItemData(d1.Item, d1.Count, isEquipment_2);
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