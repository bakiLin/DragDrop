using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryItem _inventoryItem;

    private TooltipManager _tooltipManager;

    private void Awake()
    {
        _inventoryItem = GetComponent<InventoryItem>();
        _tooltipManager = TooltipManager.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_inventoryItem.ItemSO != null)
            _tooltipManager.EnableTooltip(_inventoryItem.ItemSO.Description, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_inventoryItem.ItemSO != null)
            _tooltipManager.DisableTooltip();
    }
}
