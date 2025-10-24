using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    private InventoryManager _inventoryManager;

    private Image _image;

    private int _id;

    public int Id { get => _id; set => _id = value; }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _inventoryManager = GetComponent<InventoryManager>();
    }

    private void Start()
    {
        Deselect();
    }

    public void Initialize(int id, InventoryManager inventoryManager)
    {
        _id = id;
        _inventoryManager = inventoryManager;
    }

    public void Select()
    {
        _image.color = Color.blue;
    }

    public void Deselect()
    {
        _image.color= Color.black;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (transform.childCount == 0)
        {
            if (inventoryItem != null) 
                inventoryItem.Parent = transform;
        }
        else
        {
            var slotItem = transform.GetChild(0).GetComponent<InventoryItem>();
            if (inventoryItem.ItemSO.ItemType == slotItem.ItemSO.ItemType)
            {
                slotItem.Count += inventoryItem.Count;
                inventoryItem.Count = slotItem.Count - 5;
                if (slotItem.Count > 5) slotItem.Count = 5;

                slotItem.RefreshCount();
                inventoryItem.RefreshCount();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventoryManager.SelectSlot(_id);
        Select();
    }
}
