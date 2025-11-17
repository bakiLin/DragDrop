using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemSO", fileName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public int Id => GetInstanceID();

    [field: SerializeField] 
    public bool IsStackable { get; set; }

    [field: SerializeField]
    [field: Range(1, 100)]
    public int MaxStackSize { get; set; }

    [field: SerializeField]  
    public string Name { get; set; }

    [field: SerializeField]
    [field: TextArea] 
    public string Description { get; set; }

    [field: SerializeField] 
    public Sprite Sprite { get; set; }
}
