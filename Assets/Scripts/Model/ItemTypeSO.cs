using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemTypeSO", fileName = "ItemTypeSO")]
public class ItemTypeSO : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public ItemSO[] Items { get; private set; }

    [field: SerializeField]
    public Sprite BackgroundSprite { get; private set; }
}
