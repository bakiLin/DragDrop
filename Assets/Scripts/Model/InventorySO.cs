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

    public event Action OnInventoryDataChanged;

    private List<InventoryItemData> _itemDataList;

    public void Init()
    {
        _itemDataList = new List<InventoryItemData>();
        for (int i = 0; i < Size; i++)
            _itemDataList.Add(new InventoryItemData());
    }

    public int AddItem(ItemSO item, int count)
    {
        if (!item.IsStackable)
        {
            while (count > 0 && !IsInventoryFull())
                count -= AddItemToFirstFreeSlot(item, 1);
            OnInventoryDataChanged?.Invoke();
            return count;
        }

        count = AddStackableItem(item, count);
        OnInventoryDataChanged?.Invoke();
        return count;
    }

    public void RemoveItem(int index, int count)
    {
        if (_itemDataList.Count > index)
        {
            if (_itemDataList[index].IsEmpty) return;

            int itemCountLeft = _itemDataList[index].Count - count;
            if (itemCountLeft <= 0)
                _itemDataList[index] = new InventoryItemData();
            else
                _itemDataList[index] = new InventoryItemData(_itemDataList[index].Item, itemCountLeft);

            OnInventoryDataChanged?.Invoke();
        }
    }

    private int AddItemToFirstFreeSlot(ItemSO item, int count)
    {
        for (int i = 0; i < _itemDataList.Count; i++)
        {
            if (_itemDataList[i].IsEmpty)
            {
                _itemDataList[i] = new InventoryItemData(item, count);
                return count;
            }
        }
        return 0;
    }

    private bool IsInventoryFull()
    {
        return !_itemDataList.Where(item => item.IsEmpty).Any();
    }

    private int AddStackableItem(ItemSO item, int count)
    {
        for (int i = 0; i < _itemDataList.Count; i++)
        {
            if (_itemDataList[i].IsEmpty) continue;
            if (_itemDataList[i].Item.Id == item.Id)
            {
                int numToFull = _itemDataList[i].Item.MaxStackSize - _itemDataList[i].Count;

                if (count > numToFull)
                {
                    _itemDataList[i] = new InventoryItemData(_itemDataList[i].Item, _itemDataList[i].Item.MaxStackSize);
                    count -= numToFull;
                }
                else
                {
                    _itemDataList[i] = new InventoryItemData(_itemDataList[i].Item, _itemDataList[i].Count + count);
                    return 0;
                }
            }
        }

        while (count > 0 && !IsInventoryFull())
        {
            int newCount = Mathf.Clamp(count, 0, item.MaxStackSize);
            count -= newCount;
            AddItemToFirstFreeSlot(item, newCount);
        }

        return count;
    }

    public Dictionary<int, InventoryItemData> GetInventoryState()
    {
        Dictionary<int, InventoryItemData> dictionary = new();
        for (int i = 0; i < _itemDataList.Count; i++)
        {
            if (_itemDataList[i].IsEmpty) continue;
            dictionary[i] = _itemDataList[i];
        }
        return dictionary;
    }

    public InventoryItemData GetItemAt(int index)
    {
        return _itemDataList[index];
    }

    public void SwapItems(int index_1, int index_2)
    {
        InventoryItemData item = _itemDataList[index_1];
        _itemDataList[index_1] = _itemDataList[index_2];
        _itemDataList[index_2] = item;

        OnInventoryDataChanged?.Invoke();
    }
}