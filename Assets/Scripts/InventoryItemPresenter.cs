using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPresenter : MonoBehaviour
{
    private InventoryItem _inventoryItem;

    private TextMeshProUGUI _countText;

    private Image _image;

    private void Awake()
    {
        _inventoryItem = GetComponent<InventoryItem>();
        _countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _inventoryItem.OnRefreshCount += RefreshCount;
        _inventoryItem.OnSetRaycastTarget += SetRaycastTarget;
        _inventoryItem.OnSetSprite += SetSprite;
    }

    private void OnDisable()
    {
        _inventoryItem.OnRefreshCount -= RefreshCount;
        _inventoryItem.OnSetRaycastTarget -= SetRaycastTarget;
        _inventoryItem.OnSetSprite -= SetSprite;
    }

    private void RefreshCount(int value)
    {
        _countText.text = value.ToString();
        _countText.gameObject.SetActive(value > 1);
    }

    private void SetRaycastTarget(bool value) => _image.raycastTarget = value;

    private void SetSprite(Sprite sprite) => _image.sprite = sprite;
}
