using UnityEngine;

public class InventorySlotPresenter : MonoBehaviour
{
    private InventorySlot _inventorySlot;

    private GameObject _pointer;

    private void Awake()
    {
        _inventorySlot = GetComponent<InventorySlot>();
        _pointer = transform.Find("Image").gameObject;
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

    private void Select() => _pointer.SetActive(true);

    private void Deselect() => _pointer.SetActive(false);
}
