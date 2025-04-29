using Services;
using UnityEngine;
using Zenject;

namespace Context.SceneContexts
{
    public class GameContext : MonoInstaller
    {
        [SerializeField] private ScreenService _screenServicePrefab;
        public override void InstallBindings()
        {
            Container.Bind<ScreenService>().FromComponentInNewPrefab(_screenServicePrefab).AsCached();
            Container.BindInterfacesAndSelfTo<GameCoreService>().FromNew().AsSingle().NonLazy();
        }

        private void voidName()
        {
            // new InputSystem_Actions().Player.Jump
        }
    }
}