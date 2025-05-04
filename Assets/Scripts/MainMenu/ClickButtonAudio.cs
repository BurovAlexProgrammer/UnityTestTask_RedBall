using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    [RequireComponent(typeof(Button))]
    public class ClickButtonAudio : MonoBehaviour
    {
        [Inject] private GameAudioService _audioService;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            _audioService.PlaySfx("click");
        }
    }
}