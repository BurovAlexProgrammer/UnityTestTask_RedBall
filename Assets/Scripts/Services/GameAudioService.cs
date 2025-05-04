using AppCoreModule.Scripts.Services;
using Settings;
using Zenject;

namespace Services
{
    public class GameAudioService
    {
        [Inject] private AudioService _audioService;
        [Inject] private GameSettings _gameSettings;
        
        public void PlayMusic(string musicKey)
        {
            var audioEvent =  _gameSettings.GetMusic(musicKey);

            _audioService.PlayMusic(audioEvent);
        }
        
        public void PlaySfx(string sfxKey)
        {
            var audioEvent =  _gameSettings.GetAudioEvent(sfxKey);

            _audioService.PlaySfx(audioEvent);
        }
    }
}