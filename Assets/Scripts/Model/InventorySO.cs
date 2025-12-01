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

    private List<InventoryItemData> _itemDataList;

    public void Init()
    {
        _itemDataList = Enumerable.Range(0, Size)
            .Select(_ => new InventoryItemData()).ToList();
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
        (_itemDataList[i1], _itemDataList[i2]) = (_itemDataList[i2], _itemDataList[i1]);
        OnInventoryDataChanged?.Invoke();
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
        {
            _itemDataList[index] = new InventoryItemData(_itemDataList[index].Item,
                _itemDataList[index].Count + 1);
            return;
        }

        AddItemToFirstFreeSlot(item);
    }
}