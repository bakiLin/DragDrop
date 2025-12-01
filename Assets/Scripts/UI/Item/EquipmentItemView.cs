using UnityEngine;

public class EquipmentItemView : ItemView
{
    public override void SetData(Sprite sprite, int count)
    {
        _itemImage.sprite = sprite;
        _itemImage.gameObject.SetActive(true);
    }
}
