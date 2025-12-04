using System;
using UnityEngine;

public interface IItemController
{
    public event Action<IItemView> OnItemBeginDrag, OnItemEndDrag, OnItemDropped,
    OnItemClicked, OnPointerEntered, OnPointerExited;

    public Vector2 Position { get; }
}
