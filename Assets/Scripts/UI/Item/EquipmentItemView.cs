using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemView : ItemView
{
    [SerializeField]
    private Image _backgroundImage;

    public override void SetData(Sprite sprite, int count)
    {
        _itemImage.sprite = sprite;
        _itemImage.gameObject.SetActive(true);
    }

    public void SetBackground(Sprite sprite)
    {
        _backgroundImage.sprite = sprite;
    }
}
