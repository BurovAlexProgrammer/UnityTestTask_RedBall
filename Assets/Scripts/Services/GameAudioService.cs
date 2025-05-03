using System.Collections.Generic;
using AppCoreModule.Scripts.Audio;
using AppCoreModule.Scripts.Services;
using Settings;
using UnityEngine;
using Zenject;

namespace Services
{
    public class GameAudioService
    {
        [Inject] private AudioService _audioService;
        [Inject] private GameSettings _gameSettings;
        
        public void PlaySfx(string sfxKey)
        {
            var audioEvent =  _gameSettings.GetAudioEvent(sfxKey);

            _audioService.PlaySfx(audioEvent);
        }
    }
}