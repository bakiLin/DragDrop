using UnityEngine;
using UnityEngine.UI;

public class InventorySlotPresenter : MonoBehaviour
{
    private InventorySlot _inventorySlot;

    private Image _image;

    private void Awake()
    {
        _inventorySlot = GetComponent<InventorySlot>();
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _inventorySlot.OnSelect += Select;
        _inventorySlot.OnDeselect += Deselect;
    }

    private void OnDisable()
    {
        _inventorySlot.OnSelect -= Select;
        _inventorySlot.OnDeselect -= Deselect;
    }

    private void Select()
    {
        _image.color = Color.blue;
    }

    private void Deselect()
    {
        _image.color = Color.black;
    }
}
