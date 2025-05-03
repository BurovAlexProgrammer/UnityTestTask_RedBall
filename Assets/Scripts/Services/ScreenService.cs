using AppCoreModule.Scripts.UI.Screens;
using AppCoreModule.Scripts.UI.TransitEffects.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Services
{
    public class ScreenService : AppCoreModule.Scripts.Services.ScreenService
    {
        [Inject] private DiContainer _diContainer;

        private Transform _screenContainer;

        public override void Init(bool fadeInOnAwake, TransitEffectSettings transitEffectSettings = default)
        {
            gameObject.AddComponent<RectTransform>();
            base.Init(fadeInOnAwake, transitEffectSettings);
            _screenContainer = _screenCanvas.transform;
        }

        protected override BaseScreen InstantiateScreen(BaseScreen screenPrefab)
        {
            var screen = _diContainer.InstantiatePrefab(screenPrefab, _screenContainer);
            
            return screen.GetComponent<BaseScreen>();
        }

        public async UniTask<T> LoadScreenAsync<T>(SCREEN_ADDRESS screenAddress) where T : BaseScreen
        {
            var address = screenAddress.ToString();
            var resource = await Addressables.LoadAssetAsync<GameObject>(address);
            var screen = resource.GetComponent<T>();

            return screen;
        }
        
        public async UniTask OpenScreenAsync(SCREEN_ADDRESS screenAddress, bool clearPrevScreens = false)
        {
            var screen = await LoadScreenAsync<BaseScreen>(screenAddress);
            await OpenScreenAsync(screen, clearPrevScreens);
        }
        
        public async UniTask OpenScreenAsync(BaseScreen screen, bool clearPrevScreens = false)
        {
            await base.OpenScreenAsync(screen);
        }
    }

    //Screen name should be the same as addressable name !
    public enum SCREEN_ADDRESS
    {
        MainMenu,
        GameScreen,
        GameResultPopup
    }
}