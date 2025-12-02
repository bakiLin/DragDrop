using System;

[Serializable]
public struct InventoryItemData
{
    public ItemSO Item;

    public int Count;

    public bool IsEquipment;

    public bool IsEmpty => Item == null;

    public InventoryItemData(ItemSO item = null, int count = 0, bool isEquipment = false)
    {
        Item = item;
        Count = count;
        IsEquipment = isEquipment;
    }
}
