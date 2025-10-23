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
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.Parent = transform;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventoryManager.SelectSlot(_id);
        Select();
    }
}
