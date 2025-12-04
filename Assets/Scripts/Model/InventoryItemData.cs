using System;

[Serializable]
public struct InventoryItemData
{
    public ItemSO Item;

    public int Count;

    public bool IsEquipment;

    public bool IsEmpty => Item == null;

    public InventoryItemData(ItemSO item, int count)
    {
        Item = item;
        Count = count;
        IsEquipment = false;
    }

    public InventoryItemData(bool isEquipment)
    {
        Item = null;
        Count = 0;
        IsEquipment = isEquipment;
    }

    public InventoryItemData(ItemSO item, int count, bool isEquipment)
    {
        Item = item;
        Count = count;
        IsEquipment = isEquipment;
    }

    public InventoryItemData(InventoryItemData data)
    {
        Item = data.Item;
        Count = data.Count;
        IsEquipment = data.IsEquipment;
    }
}
