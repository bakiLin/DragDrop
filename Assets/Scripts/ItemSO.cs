using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item", fileName = "Item")]
public class ItemSO : ScriptableObject
{
    public Sprite Sprite;
    public string Description;
    public ItemType ItemType;
    public bool Stackable;
}

public enum ItemType {
    Weapon, 
    Potion,
    QuestItem 
}
