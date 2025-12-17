using TMPro;
using UnityEngine;

public class InventoryItemView : ItemView
{
    [SerializeField]
    private TextMeshProUGUI _itemCountText;

    public override void SetData(Sprite sprite, int count)
    {
        _itemCountText.text = count == 1 ? "" : count.ToString();
        _itemImage.sprite = sprite;
        _itemImage.gameObject.SetActive(true);
    }
}
