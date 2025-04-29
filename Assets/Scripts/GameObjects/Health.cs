using UniRx;
using UnityEngine;

namespace GameObjects
{
    public class Health
    {
        private IReadOnlyReactiveProperty<bool> _isDead;
        private readonly ReactiveProperty<int> _maxHp = new();
        private readonly ReactiveProperty<int> _currHp = new();

        public IReadOnlyReactiveProperty<int> CurrHp => _currHp;
        public IReadOnlyReactiveProperty<int> MaxHp => _maxHp;
        
        public void Init(int currHp, int maxHp)
        {
            _currHp.Value = currHp;
            _maxHp.Value = maxHp;
            _isDead = _currHp.Select(x => x <= 0).ToReactiveProperty<bool>();
        }
        
        public void DealDamage(int damage)
        {
            if (damage <= 0 || _isDead.Value) return;

            _currHp.Value = Mathf.Clamp(_currHp.Value - damage, 0, int.MaxValue);
        }
    }
}