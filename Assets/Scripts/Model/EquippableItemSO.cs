using UnityEngine;

[CreateAssetMenu(menuName = "SO/EquippableItemSO", fileName = "EquippableItemSO")]
public class EquippableItemSO : ItemSO
{
    public override void Interact(InventorySO data, int index)
    {
        data.EquipItem(index);
    }
}
