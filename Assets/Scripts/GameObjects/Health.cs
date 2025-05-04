using UniRx;
using UnityEngine;

namespace GameObjects
{
    public class Health
    {
        public IReadOnlyReactiveProperty<bool> IsDead => _currHp.Select(x => x <= 0).ToReactiveProperty<bool>();
        private readonly ReactiveProperty<int> _maxHp = new() {Value = 1};
        private readonly ReactiveProperty<int> _currHp = new() {Value = 1};

        public IReadOnlyReactiveProperty<int> CurrHp => _currHp;
        public IReadOnlyReactiveProperty<int> MaxHp => _maxHp;
        
        public void Init(int currHp, int maxHp)
        {
            _currHp.Value = currHp;
            _maxHp.Value = maxHp;
        }
        
        public void DealDamage(int damage, bool withKnockBack = true)
        {
            if (damage <= 0 || IsDead.Value) return;

            _currHp.Value = Mathf.Clamp(_currHp.Value - damage, 0, int.MaxValue);
        }
    }
}