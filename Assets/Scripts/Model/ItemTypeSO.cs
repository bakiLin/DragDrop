using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemTypeSO", fileName = "ItemTypeSO")]
public class ItemTypeSO : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public Sprite EquipmentSprite { get; private set; }

    [field: SerializeField]
    public bool IsEquippable { get; private set; }

    [field: SerializeField]
    public ItemSO[] Items { get; private set; }

    public bool IsItemType(ItemSO item) => Items.ToList().Contains(item);
}
