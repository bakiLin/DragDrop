using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private int _count = 1;

    private ItemSO _itemSO;

    private Transform _parent;

    public int Count { 
        get => _count;
        set {
            _count = value;
            if (_count <= 0) Destroy(gameObject);
            OnRefreshCount?.Invoke(_count);
        }
    }
    
    public ItemSO ItemSO { get => _itemSO; set => _itemSO = value; }

    public Transform Parent { get => _parent; }

    public event Action<int> OnRefreshCount;

    public event Action<bool> OnSetRaycastTarget;

    public event Action<Sprite> OnSetSprite;

    private void Awake()
    {
        _parent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnSetRaycastTarget?.Invoke(false);
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnSetRaycastTarget?.Invoke(true);
        transform.SetParent(_parent);
        transform.localPosition = Vector2.zero;
    }

    public void SetParent(Transform parent)
    {
        _parent = parent;
        transform.SetParent(_parent);
        transform.SetAsFirstSibling();
        transform.localPosition = Vector2.zero;
    }

    public void Initialize(ItemSO item)
    {
        _itemSO = item;
        OnSetSprite?.Invoke(_itemSO.Sprite);
        OnRefreshCount?.Invoke(_count);
    }
}
