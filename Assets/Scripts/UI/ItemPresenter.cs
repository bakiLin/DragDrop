using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPresenter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
    IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<ItemPresenter> OnItemBeginDrag, OnItemEndDrag, OnItemDropped, 
        OnItemClicked, OnPointerEntered, OnPointerExited;

    [SerializeField] 
    private Image _itemImage;

    [SerializeField] 
    private TextMeshProUGUI _itemCountText;

    [SerializeField] 
    private Image _borderImage;

    private bool _isEmpty = true;

    private void Awake()
    {
        ResetData();
        SelectItem(false);
    }

    public void ResetData()
    {
        _itemImage.gameObject.SetActive(false);
        _isEmpty = true;
    }

    public void SelectItem(bool isActive)
    {
        _borderImage.enabled = isActive;
    }

    public void SetData(Sprite sprite, int count)
    {
        _itemCountText.text = count.ToString();
        _itemImage.sprite = sprite;
        _itemImage.gameObject.SetActive(true);
        _isEmpty = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isEmpty) return;
        OnItemBeginDrag?.Invoke(this);
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
        if (_isEmpty) return;
        OnPointerEntered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isEmpty) return;
        OnPointerExited?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData) { }
}
