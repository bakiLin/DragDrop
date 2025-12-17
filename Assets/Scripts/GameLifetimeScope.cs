using VContainer;
using VContainer.Unity;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField]
    private InventorySO _inventoryData;

    [SerializeField]
    private InventoryView _inventoryView;

    [SerializeField]
    private ItemController _itemPrefab;

    [SerializeField]
    private Tooltip _tooltip;

    [SerializeField]
    private PointerFollower _pointerFollower;

    [SerializeField]
    private ItemDropView _itemDropView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_inventoryData);
        builder.RegisterInstance(_inventoryView);
        builder.RegisterInstance(_itemPrefab);
        builder.RegisterInstance(_tooltip);
        builder.RegisterInstance(_pointerFollower);
        builder.RegisterInstance(_itemDropView);
    }
}
