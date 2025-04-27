using Services;
using Zenject;

namespace Context.SceneContexts
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputService>().FromNew().AsSingle().NonLazy();
        }
    }
}