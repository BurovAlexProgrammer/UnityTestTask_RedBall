using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace GameObjects
{
    public class Health
    {
        public IReadOnlyReactiveProperty<bool> IsDead => _currHp.Select(x => x <= 0).ToReactiveProperty<bool>();
        public readonly ReactiveProperty<bool> IsKnockedBack = new();
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
        
        public void ApplyKnockBack(Rigidbody2D rigidbody, Vector2 direction, float knockbackForce)
        {
            rigidbody.linearVelocity = Vector2.zero;
            rigidbody.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            IsKnockedBack.Value = true;
            ResetKnockbackState(0.5f).Forget();
        }

        private async UniTask ResetKnockbackState(float delayInSeconds)
        {
            await UniTask.WaitForSeconds(delayInSeconds);
            IsKnockedBack.Value = false;
        }
    }
}