using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IItemController, IItemView, IBeginDragHandler, IDragHandler, IEndDragHandler, 
    IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<IItemView> OnItemBeginDrag, OnItemEndDrag, OnItemDropped, 
        OnItemClicked, OnPointerEntered, OnPointerExited;

    public Vector2 Position => _rectTransform.position;

    private ItemView _itemView;

    private bool _isEmpty = true;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _itemView = GetComponent<ItemView>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void ResetData()
    {
        _isEmpty = true;
        _itemView.ResetData();
    }

    public void SetData(Sprite sprite, int count)
    {
        _isEmpty = false;
        _itemView.SetData(sprite, count);
    }

    public void ToggleItem(bool isActive)
    {
        _itemView.ToggleItem(isActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_isEmpty) OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDropped?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnItemClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isEmpty) OnPointerEntered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isEmpty) OnPointerExited?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData) { }
}
