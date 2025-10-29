using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Inject] private TooltipManager _tooltipManager;

    private InventoryItem _inventoryItem;

    private void Awake()
    {
        _inventoryItem = GetComponent<InventoryItem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltipManager.EnableTooltip(_inventoryItem.ItemSO.Description, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltipManager.DisableTooltip();
    }
}
