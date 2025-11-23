using System;
using System.Collections.Generic;
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

            if (_itemDataList[index].Count > 1)
                _itemDataList[index] = new InventoryItemData(_itemDataList[index].Item, _itemDataList[index].Count - 1);
            else
                _itemDataList[index] = new InventoryItemData();

            OnInventoryDataChanged?.Invoke();
        }
    }

    public Dictionary<int, InventoryItemData> GetInventory()
    {
        Dictionary<int, InventoryItemData> dictionary = new();
        for (int i = 0; i < _itemDataList.Count; i++)
        {
            if (_itemDataList[i].IsEmpty) continue;
            dictionary[i] = _itemDataList[i];
        }
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
        for (int i = 0; i < _itemDataList.Count; i++)
        {
            if (_itemDataList[i].IsEmpty)
            {
                _itemDataList[i] = new InventoryItemData(item, 1);
                return;
            }
        }
    }

    private void AddStackableItem(ItemSO item)
    {
        for (int i = 0; i < _itemDataList.Count; i++)
        {
            if (_itemDataList[i].IsEmpty)
                continue;
            if (_itemDataList[i].Item.Id == item.Id)
            {
                if (MaxStackSize - _itemDataList[i].Count > 0)
                {
                    _itemDataList[i] = new InventoryItemData(_itemDataList[i].Item, _itemDataList[i].Count + 1);
                    return;
                }
            }
        }

        AddItemToFirstFreeSlot(item);
    }
}