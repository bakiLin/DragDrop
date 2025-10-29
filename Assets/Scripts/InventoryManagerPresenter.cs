using UnityEngine;
using UnityEngine.UI;

public class InventoryManagerPresenter : MonoBehaviour
{
    [SerializeField] private Button _drop, _confirmDrop, _sort;

    private InventoryManager _inventoryManager;

    private void Awake()
    {
        _inventoryManager = GetComponent<InventoryManager>();
    }

    private void Start()
    {
        _drop.onClick.AddListener(delegate { _inventoryManager.DropItem(); });
        _confirmDrop.onClick.AddListener(delegate { _inventoryManager.ConfirmDrop(); });
        _sort.onClick.AddListener(delegate { _inventoryManager.Sort(); });
    }
}
