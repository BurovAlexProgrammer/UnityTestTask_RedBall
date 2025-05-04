using System;
using Cysharp.Threading.Tasks;
using GameObjects;
using UniRx;
using Zenject;

namespace Services
{
    public class GameCoreService : IInitializable, IDisposable
    {
        [Inject] private PlayerController _playerController;
        [Inject] private InputService _inputService;
        [Inject] private ScreenService _screenService;
        
        private IDisposable _subscribeIsDead;


        public void Initialize()
        {
            _inputService.PlayerActions.Enable();
            _subscribeIsDead = _playerController.Health.IsDead.Subscribe(OnDead);
            _playerController.Finished += OnPlayerFinished;
        }

        private void OnPlayerFinished()
        { 
            GameOver(true).Forget();
        }

        public void Dispose()
        {
            _subscribeIsDead.Dispose();
            _playerController.Finished -= OnPlayerFinished;
        }

        public async UniTask GameOver(bool isWin)
        {
            _inputService.PlayerActions.Disable();
            var popupPrefab = await _screenService.LoadScreenAsync<GameResultPopup>(SCREEN_ADDRESS.GameResultPopup);
            _screenService.OpenScreenAsync(popupPrefab).Forget();
            _screenService.GetCurrentScreen().GetComponent<GameResultPopup>().Init(isWin);
        }

        private void OnDead(bool isDead)
        {
            if (isDead)
                GameOver(false).Forget();
        }
    }
}