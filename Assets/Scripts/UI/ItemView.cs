using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IItemView
{
    [SerializeField]
    private Image _itemImage;

    [SerializeField]
    private Image _selectionImage;

    [SerializeField]
    private TextMeshProUGUI _itemCountText;

    public void ResetData()
    {
        _itemImage.gameObject.SetActive(false);
    }

    public void SetData(Sprite sprite, int count)
    {
        _itemCountText.text = count == 1 ? "" : count.ToString();
        _itemImage.sprite = sprite;
        _itemImage.gameObject.SetActive(true);
    }

    public void ToggleItem(bool isActive)
    {
        _selectionImage.enabled = isActive;
    }
}
