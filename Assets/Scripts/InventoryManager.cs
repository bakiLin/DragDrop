using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private DropManager _dropManager;

    [SerializeField] private int _maxStack;

    [SerializeField] private float _doubleClickTime;

    [SerializeField] private GameObject _inventoryItemPrefab;

    [SerializeField] private DropdownManager _dropdownManager;

    [SerializeField] private ItemType[] _sortOrder;

    [SerializeField] private InventorySlot[] InventorySlots;

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
        for (int i = 0; i < InventorySlots.Length; i++)
            InventorySlots[i].Id = i;
    }

    public void AddItem()
    {
        var item = _dropdownManager.GetSelectedItem();

        if (item != null)
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                var slotItem = InventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (slotItem != null && item.Stackable && slotItem.ItemSO == item && slotItem.Count < _maxStack)
                {
                    slotItem.Count++;
                    return;
                }
            }

            for (int i = 0; i < InventorySlots.Length; i++)
            {
                var slot = InventorySlots[i];
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
            var slotItem = InventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
            if (slotItem != null)
            {
                if (slotItem.Count == 1) Destroy(slotItem.gameObject);
                else _dropManager.InitializeSlider(slotItem.Count);
            }
        }
    }

    public void ConfirmDrop()
    {
        var slotItem = InventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();
        slotItem.Count -= _dropManager.GetSliderValue();
    }

    public void SelectSlot(int id)
    {
        if (_selectedSlot >= 0) InventorySlots[_selectedSlot].Deselect();
        if (id == _selectedSlot && Time.time - _lastClickTime < _doubleClickTime) DoubleClick();

        _selectedSlot = id;
        _lastClickTime = Time.time;
    }

    private void DoubleClick()
    {
        print("double click");
    }

    public void Sort()
    {
        Transform[] items = new Transform[InventorySlots.Length];
        int orderCounter = 0;
        int itemCounter = 0;

        while (orderCounter < _sortOrder.Length)
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                Transform child = null;
                if (InventorySlots[i].transform.childCount == 2)
                    child = InventorySlots[i].transform.GetChild(1);

                if (child != null && child.GetComponent<InventoryItem>().ItemSO.ItemType == _sortOrder[orderCounter])
                {
                    items[itemCounter] = child;
                    itemCounter++;
                }
            }
            orderCounter++;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].GetComponent<InventoryItem>().SetParent(InventorySlots[i].transform);
                items[i].localPosition = Vector3.zero;
            }
        }
    }
}
