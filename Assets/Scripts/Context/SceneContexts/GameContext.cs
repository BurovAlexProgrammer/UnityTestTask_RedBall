using Services;
using UnityEngine.InputSystem;
using Zenject;

namespace Context.SceneContexts
{
    public class GameContext : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            
        }

        private void voidName()
        {
            // new InputSystem_Actions().Player.Jump
        }
    }
}