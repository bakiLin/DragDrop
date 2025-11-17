using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/InventorySO", fileName = "InventorySO")]
public class InventorySO : ScriptableObject
{
    [field: SerializeField]
    [field: Range(1, 20)]
    public int Size { get; private set; }

    private List<InventoryItemData> _itemDataList;

    public void Init()
    {
        _itemDataList = new List<InventoryItemData>();
        for (int i = 0; i < Size; i++)
            _itemDataList.Add(new InventoryItemData());
    }

    public void AddItem(ItemSO item, int count)
    {
        for (int i = 0; i < _itemDataList.Count; i++)
        {
            if (_itemDataList[i].IsEmpty)
            {
                _itemDataList[i] = new InventoryItemData(item, count);
                return;
            }
        }
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
    }
}

[Serializable]
public struct InventoryItemData
{
    public ItemSO Item;

    public int Count;

    public bool IsEmpty => Item == null;

    public InventoryItemData(ItemSO item, int count)
    {
        Item = item;
        Count = count;
    }
}

