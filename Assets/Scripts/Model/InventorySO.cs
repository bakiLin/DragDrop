using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/InventorySO", fileName = "InventorySO")]
public class InventorySO : ScriptableObject
{
    [field: SerializeField]
    [field: Range(1, 20)]
    public int Size { get; private set; }

    [field: SerializeField]
    [field: Range(1, 100)]
    public int MaxStackSize { get; private set; }

    public event Action OnInventoryDataChanged;

    private List<InventoryItemData> _itemDataList;

    public void Init()
    {
        _itemDataList = new List<InventoryItemData>();
        for (int i = 0; i < Size; i++)
            _itemDataList.Add(new InventoryItemData());
    }

    public void AddItem(ItemSO item)
    {
        if (!item.IsStackable)
        {
            AddItemToFirstFreeSlot(item);
            OnInventoryDataChanged?.Invoke();
            return;
        }

        AddStackableItem(item);
        OnInventoryDataChanged?.Invoke();
    }

    public void RemoveItem(int index)
    {
        if (_itemDataList.Count > index)
        {
            if (_itemDataList[index].IsEmpty) return;

            _itemDataList[index] = _itemDataList[index].Count > 1 ?
                new InventoryItemData(_itemDataList[index].Item, _itemDataList[index].Count - 1) :
                _itemDataList[index] = new InventoryItemData();

            OnInventoryDataChanged?.Invoke();
        }
    }

    public Dictionary<int, InventoryItemData> GetInventory()
    {
        var dictionary = _itemDataList
            .Select((data, index) => (data, index))
            .Where(x => !x.data.IsEmpty)
            .ToDictionary(x => x.index, x => x.data);
        return dictionary;
    }

    public void SwapItems(int index_1, int index_2)
    {
        InventoryItemData item = _itemDataList[index_1];
        _itemDataList[index_1] = _itemDataList[index_2];
        _itemDataList[index_2] = item;
        OnInventoryDataChanged?.Invoke();
    }

    public InventoryItemData GetItemAt(int index) => _itemDataList[index];

    private void AddItemToFirstFreeSlot(ItemSO item)
    {
        int index = _itemDataList.FindIndex(x => x.IsEmpty);
        if (index >= 0) _itemDataList[index] = new InventoryItemData(item, 1);
    }

    private void AddStackableItem(ItemSO item)
    {
        int index = _itemDataList.FindIndex(x => !x.IsEmpty && x.Item.Id == item.Id && MaxStackSize - x.Count > 0);
        if (index >= 0)
            _itemDataList[index] = new InventoryItemData(_itemDataList[index].Item, _itemDataList[index].Count + 1);
        else
            AddItemToFirstFreeSlot(item);
    }
}