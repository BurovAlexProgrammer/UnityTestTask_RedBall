using AppCoreModule.Scripts.Services;
using Services;
using Settings;
using UnityEngine;
using Zenject;

namespace Context.SceneContexts
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private AudioService _audioServiceInstance;
        [SerializeField] private GameSettings _gameSettingsInstance;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AudioService>().FromInstance(_audioServiceInstance).AsSingle();
            Container.BindInterfacesAndSelfTo<GameSettings>().FromInstance(_gameSettingsInstance).AsSingle();
            Container.BindInterfacesAndSelfTo<GameAudioService>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputService>().FromNew().AsSingle().NonLazy();
        }
    }
}