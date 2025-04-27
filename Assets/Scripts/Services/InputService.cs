using Zenject;

namespace Services
{
    public class InputService : IInitializable
    {
        private InputSystem_Actions _inputActions;

        public InputSystem_Actions.PlayerActions PlayerActions => _inputActions.Player;
        public InputSystem_Actions.UIActions UiActions => _inputActions.UI;
        
        public void Initialize()
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Player.Enable();
        }
    }
}