using System;

public interface IItemController
{
    public event Action<IItemView> OnItemBeginDrag, OnItemEndDrag, OnItemDropped,
    OnItemClicked, OnPointerEntered, OnPointerExited;
}
