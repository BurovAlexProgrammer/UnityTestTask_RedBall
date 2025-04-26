using Services;
using UnityEngine;
using Zenject;

namespace Context.SceneContexts
{
    public class MainMenuContext: MonoInstaller
    {
        [SerializeField] private ScreenService _screenServicePrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<ScreenService>().FromComponentInNewPrefab(_screenServicePrefab).AsCached();
        }
    }
}