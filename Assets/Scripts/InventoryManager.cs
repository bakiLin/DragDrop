using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private int _maxStack;

    public InventorySlot[] InventorySlots;

    public GameObject _inventoryItemPrefab;

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
        inventoryItem.Initialize(item);
    }
}
