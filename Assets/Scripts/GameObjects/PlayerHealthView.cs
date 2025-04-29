using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameObjects
{
    public class PlayerHealthView  :MonoBehaviour
    {
        [Inject] private PlayerController _playerController;
        
        [SerializeField] private List<PlayerHealthUnitView> _hearts = new(3);
        [SerializeField] private PlayerHealthUnitView _heartPrefab;
        
        private IDisposable _subscribeMaxHp;
        private IDisposable _subscribeCurrHp;

        private void Start()
        {
            _subscribeMaxHp = _playerController.Health.MaxHp.Subscribe(OnMaxHpChanged);
            _subscribeCurrHp = _playerController.Health.CurrHp.Subscribe(HealthOnHpChanged);
        }

        private void OnDestroy()
        {
            _subscribeMaxHp.Dispose();
            _subscribeCurrHp.Dispose();
        }

        private void OnMaxHpChanged(int maxHp)
        {
            ClearHearts();
            CreateHearts(maxHp);
        }
        
        private void HealthOnHpChanged(int currenthp)
        {
            for (var i = 0; i < _hearts.Count; i++)
            {
                var heart = _hearts[i];
                heart.SetFill(currenthp >= i + 1);
            }
        }

        private void CreateHearts(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var newHeart = Instantiate(_heartPrefab, transform);
                _hearts.Add(newHeart);
            }
        }
        
        private void ClearHearts()
        {
            foreach (var heart in _hearts)
            {
                Destroy(heart.gameObject);
            }
            
            _hearts.Clear();
        }
    }
}