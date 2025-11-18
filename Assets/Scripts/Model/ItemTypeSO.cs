using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemTypeSO", fileName = "ItemTypeSO")]
public class ItemTypeSO : ScriptableObject
{
    public string Name;

    [field: SerializeField]
    public ItemSO[] Items { get; private set; }
}
