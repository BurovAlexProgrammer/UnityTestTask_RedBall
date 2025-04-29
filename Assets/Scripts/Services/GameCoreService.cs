using Zenject;

namespace Services
{
    public class GameCoreService : IInitializable
    {
        [Inject] private PlayerController _playerController;


        public void Initialize()
        {
            var t = _playerController;
        }
    }
}