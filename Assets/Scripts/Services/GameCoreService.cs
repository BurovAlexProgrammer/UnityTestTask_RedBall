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
        }

        public void Dispose()
        {
            _subscribeIsDead.Dispose();
        }

        private void OnDead(bool isDead)
        {
            if (isDead)
                GameOver().Forget();
        }

        private async UniTask GameOver()
        {
            _inputService.PlayerActions.Disable();
            var popup = await _screenService.LoadScreenAsync<GameResultPopup>(SCREEN_ADDRESS.GameResultPopup);
            _screenService.OpenScreenAsync(popup).Forget();
            popup.Setup(false);
        }
    }
}