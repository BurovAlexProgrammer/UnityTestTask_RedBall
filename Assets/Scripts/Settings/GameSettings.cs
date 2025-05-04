using System;
using System.Collections.Generic;
using AppCoreModule.Scripts.Audio;
using UnityEngine;
using Zenject;

namespace Settings
{
    [CreateAssetMenu(menuName = "GameCore/GameSettings")]
    public class GameSettings : ScriptableObject, IInitializable
    {
        [SerializeField] public List<AudioEventPair> AudioEvents = new();
        [SerializeField] public List<AudioEventPair> Musics = new();

        private Dictionary<string, AudioEventPair> AudioEventsDic = new();
        private Dictionary<string, AudioEventPair> MusicsDic = new();
        
        public AudioEvent GetMusic(string key)
        {
            if (MusicsDic.TryGetValue(key, out var musicPair) == false)
            {
                Debug.LogError($"GameSettings: AudioEvent with key [{key}] not found.");
                return null;
            }

            return musicPair.AudioEvent;
        }
        
        public AudioEvent GetAudioEvent(string key)
        {
            if (AudioEventsDic.TryGetValue(key, out var audioEventPair) == false)
            {
                Debug.LogError($"GameSettings: AudioEvent with key [{key}] not found.");
                return null;
            }

            return audioEventPair.AudioEvent;
        }
        
        [Serializable]
        public class AudioEventPair
        {
            public string Key;
            public AudioEvent AudioEvent;
        }

        public void Initialize()
        {
            AudioEventsDic = new(AudioEvents.Count);
            
            foreach (var audioEventPair in AudioEvents)
            {
                if (AudioEventsDic.TryAdd(audioEventPair.Key, audioEventPair) == false)
                {
                    Debug.LogError("GameSettings: AudioEvent with key [" + audioEventPair.Key + "] already registered. Overrided.");
                }
            }

            MusicsDic = new (Musics.Count);
            
            foreach (var musicPair in Musics)
            {
                if (MusicsDic.TryAdd(musicPair.Key, musicPair) == false)
                {
                    Debug.LogError("GameSettings: Music with key [" + musicPair.Key + "] already registered. Overrided.");
                }
            }
        }
    }
}