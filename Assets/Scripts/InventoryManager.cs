using UnityEngine;
using UnityEngine.EventSystems;

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

    [SerializeField]
    private ItemType[] _sortOrder;

    private int _selectedSlot = -1;

    private float _lastClickTime;

    public static InventoryManager Instance;

    //private InventorySlot _selectedInventorySlot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < InventorySlots.Length; i++)
            InventorySlots[i].GetComponent<InventorySlot>().Id = i;
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
                if (InventorySlots[i].transform.childCount == 1)
                    child = InventorySlots[i].transform.GetChild(0);

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

    public void ConfirmDrop()
    {
        InventoryItem slotItem = InventorySlots[_selectedSlot].GetComponentInChildren<InventoryItem>();

        slotItem.Count -= _dropManager.GetSliderValue();
        if (slotItem.Count == 0) Destroy(slotItem.gameObject);
        //else slotItem.RefreshCount();
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

    public void SelectSlot(int id/*int value*/)
    {

        if (_selectedSlot >= 0) InventorySlots[_selectedSlot].Deselect();
        if (id == _selectedSlot && Time.time - _lastClickTime < _doubleClickTime)
            DoubleClick();

        _selectedSlot = id;
        //_selectedInventorySlot = 
        _lastClickTime = Time.time;
        InventorySlots[_selectedSlot].Select();
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
                //slotItem.RefreshCount();
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
