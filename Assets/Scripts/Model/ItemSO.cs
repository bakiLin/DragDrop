using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public int Id => GetInstanceID();

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }

    [field: SerializeField]
    public Sprite Sprite { get; private set; }

    [field: SerializeField]
    public bool IsStackable { get; private set; }

    public abstract void Interact(InventorySO data, int index);
}
