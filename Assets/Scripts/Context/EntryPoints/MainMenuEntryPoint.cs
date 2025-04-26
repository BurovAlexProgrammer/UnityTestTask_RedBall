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
        
        private void Start()
        {
            _screenService.Init(true);
            _screenService.OpenScreenAsync(SCREEN_ADDRESS.MainMenu).Forget();
        }
    }
}