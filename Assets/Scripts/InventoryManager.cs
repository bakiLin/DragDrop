using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private DropManager _dropManager;

    [SerializeField] private int _maxStack;

    [SerializeField] private float _doubleClickTime;

    [SerializeField] private GameObject _inventoryItemPrefab;

    [SerializeField] private DropdownManager _dropdownManager;

    [SerializeField] private ItemSO[] _sortOrder;

    [SerializeField] private InventorySlot[] _inventorySlots;

    [SerializeField] private Equipment[] _equipment;

    private int _selectedSlot = -1;

    private float _lastClickTime;

    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
            _inventorySlots[i].Id = i;
    }

    public void AddItem()
    {
        var item = _dropdownManager.GetSelectedItem();

        if (item != null)
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                var slotItem = _inventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (slotItem != null && item.Stackable && slotItem.ItemSO == item && slotItem.Count < _maxStack)
                {
                    slotItem.Count++;
                    return;
                }
            }

            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                var slot = _inventorySlots[i];
                if (!slot.GetComponentInChildren<InventoryItem>())
                {
                    var itemObj = Instantiate(_inventoryItemPrefab, slot.transform).GetComponent<InventoryItem>();
                    itemObj.transform.SetAsFirstSibling();
                    itemObj.AddItem(item);
                    return;
                }
            }
        }
    }

    public void DropItem()
    {
        if (_selectedSlot >= 0)
        {
            var slotItem = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
            if (slotItem != null)
            {
                if (slotItem.Count == 1) Destroy(slotItem.gameObject);
                else _dropManager.InitializeSlider(slotItem.Count);
            }
        }
    }

    public void ConfirmDrop()
    {
        var slotItem = _inventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
        slotItem.Count -= _dropManager.GetSliderValue();
    }

    public void SelectSlot(int id)
    {
        if (_selectedSlot >= 0) _inventorySlots[_selectedSlot].Deselect();
        if (id == _selectedSlot && Time.time - _lastClickTime < _doubleClickTime) DoubleClick(id);

        _selectedSlot = id;
        _lastClickTime = Time.time;
    }

    private void DoubleClick(int id)
    {
        var item = _inventorySlots[id].GetComponentInChildren<InventoryItem>();
        if (item == null) return;
        else
        {
            for (int i = 0; i < _equipment.Length; i++)
            {
                if (_equipment[i].ItemType == item.ItemSO.ItemType)
                {
                    item.SetParent(_equipment[i].Slot);
                    return;
                }
            }
        }
    }

    public void Sort()
    {
        Transform[] items = new Transform[_inventorySlots.Length];
        int orderCounter = 0;
        int itemCounter = 0;

        while (orderCounter < _sortOrder.Length)
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventoryItem child = _inventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (child != null && child.ItemSO == _sortOrder[orderCounter])
                {
                    items[itemCounter] = child.transform;
                    itemCounter++;
                }
            }
            orderCounter++;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].GetComponent<InventoryItem>().SetParent(_inventorySlots[i].transform);
                items[i].localPosition = Vector3.zero;
            }
        }
    }
}
