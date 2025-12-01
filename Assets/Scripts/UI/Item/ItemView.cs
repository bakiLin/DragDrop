using UnityEngine;
using UnityEngine.UI;

public abstract class ItemView : MonoBehaviour, IItemView
{
    [SerializeField]
    protected Image _itemImage;

    [SerializeField]
    protected Image _selectionImage;

    public virtual void ResetData() { }

    public virtual void SetData(Sprite sprite, int count) { }

    public virtual void ToggleItem(bool isActive)
    {
        _selectionImage.enabled = isActive;
    }
}