using Zenject;

public class Installer : MonoInstaller
{
    public TooltipManager TooltipManager;

    public InventoryManager InventoryManager;

    public ItemDrop ItemDrop;

    public override void InstallBindings()
    {
        Container.Bind<TooltipManager>().FromInstance(TooltipManager).AsSingle().NonLazy();

        Container.Bind<InventoryManager>().FromInstance(InventoryManager).AsSingle().NonLazy();

        Container.Bind<ItemDrop>().FromInstance(ItemDrop).AsSingle().NonLazy();
    }
}