using UnityEngine;

[CreateAssetMenu(menuName = "SO/EdibleItemSO", fileName = "EdibleItemSO")]
public class EdibleItemSO : ItemSO
{
    public override void Interact(InventorySO data, int index)
    {
        data.RemoveItem(index);
    }
}
