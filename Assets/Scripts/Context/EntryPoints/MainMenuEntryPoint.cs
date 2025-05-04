using System;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using Zenject;

namespace Context.EntryPoints
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [Inject] private ScreenService _screenService;
        [Inject] private GameAudioService _audioService;
        
        private void Start()
        {
            _audioService.PlayMusic("menuMusic");
            _screenService.Init(true);
            _screenService.OpenScreenAsync(SCREEN_ADDRESS.MainMenu).Forget();
        }
    }
}