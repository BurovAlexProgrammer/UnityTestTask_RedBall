using System;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using Zenject;

namespace Context.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [Inject] private ScreenService _screenService;
        [Inject] private PlayerController _playerController;

        private void Start()
        {
            _screenService.Init(true);
            _screenService.OpenScreenAsync(SCREEN_ADDRESS.GameScreen, true).Forget();
        }

    }
}