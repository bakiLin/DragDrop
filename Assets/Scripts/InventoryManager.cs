using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private DropManager _dropManager;

    [SerializeField]
    private int _maxStack;

    [SerializeField]
    private float _doubleClickTime;

    public GameObject _inventoryItemPrefab;

    public InventorySlot[] InventorySlots;

    private int _selectedSlot = -1;

    private float _lastClickTime;

    private void Start()
    {
        for (int i = 0; i < InventorySlots.Length; i++)
            InventorySlots[i].GetComponent<InventorySlot>().Initialize(i, this);
    }

    public void ConfirmDrop()
    {
        InventoryItem slotItem = InventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();

        slotItem.Count -= _dropManager.GetSliderValue();
        if (slotItem.Count == 0) Destroy(slotItem.gameObject);
        else slotItem.RefreshCount();
    }

    public void Drop()
    {
        if (_selectedSlot >= 0)
        {
            InventoryItem slotItem = InventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();

            if (slotItem != null)
            {
                if (slotItem.Count == 1) Destroy(slotItem.gameObject);
                else _dropManager.InitializeSlider(slotItem.Count);
            }
        }
    }

    public void SelectSlot(int value)
    {
        if (_selectedSlot >= 0) InventorySlots[_selectedSlot].Deselect();
        if (_selectedSlot == value && Time.time - _lastClickTime < _doubleClickTime)
            DoubleClick();

        _selectedSlot = value;
        _lastClickTime = Time.time;
    }

    private void DoubleClick()
    {
        print("double click");
    }

    public void AddItem(ItemSO item)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem slotItem = slot.GetComponentInChildren<InventoryItem>();

            if (slotItem != null && item.Stackable && slotItem.ItemSO == item && slotItem.Count < _maxStack)
            {
                slotItem.Count++;
                slotItem.RefreshCount();
                return;
            }
        }

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem slotItem = slot.GetComponentInChildren<InventoryItem>();

            if (slot.GetComponentInChildren<InventoryItem>() == null)
            {
                SpawnItem(item, slot);
                return;
            }
        }
    }

    private void SpawnItem(ItemSO item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(_inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.AddItem(item);
    }
}
