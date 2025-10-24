using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int Count = 1;

    [SerializeField]
    private TextMeshProUGUI _countText;

    private Image _image;
    
    // Item
    private ItemSO _itemSO;

    public ItemSO ItemSO { get => _itemSO; set => _itemSO = value; }

    // Parent
    private Transform _parent;

    public Transform Parent { get => _parent; set => _parent = value; }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _parent = transform.parent;
    }

    public void AddItem(ItemSO item)
    {
        _itemSO = item;
        _image.sprite = _itemSO.Sprite;
        RefreshCount();
    }

    public void RefreshCount()
    {
        _countText.text = Count.ToString();
        _countText.gameObject.SetActive(Count > 1);

        if (Count <= 0) Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _image.raycastTarget = false;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;
        transform.SetParent(_parent);
        transform.localPosition = Vector2.zero;
    }
}
