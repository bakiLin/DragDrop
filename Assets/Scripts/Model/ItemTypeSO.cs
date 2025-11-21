using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemTypeSO", fileName = "ItemTypeSO")]
public class ItemTypeSO : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public ItemSO[] Items { get; set; }
}
