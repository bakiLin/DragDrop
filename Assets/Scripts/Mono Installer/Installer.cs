using Zenject;

public class Installer : MonoInstaller
{
    public TooltipManager TooltipManager;

    public override void InstallBindings()
    {
        Container.Bind<TooltipManager>().FromInstance(TooltipManager).AsSingle().NonLazy();
    }
}