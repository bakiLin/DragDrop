using UnityEngine;

public interface IItemView
{
    public void ResetData();

    public void SetData(Sprite sprite, int count);

    public void ToggleItem(bool isActive);
}
