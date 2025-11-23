using System;

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
