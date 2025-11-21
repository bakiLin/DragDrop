using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public int Id => GetInstanceID();

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    [field: TextArea]
    public string Description { get; set; }

    [field: SerializeField]
    public Sprite Sprite { get; set; }

    [field: SerializeField] 
    public bool IsStackable { get; set; }

    [field: SerializeField]
    [field: Range(1, 1000)]
    public int MaxStackSize { get; set; }
}
