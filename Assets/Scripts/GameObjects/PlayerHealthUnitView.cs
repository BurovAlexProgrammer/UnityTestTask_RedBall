using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjects
{
    public class PlayerHealthUnitView : MonoBehaviour
    {
        [SerializeField] private Image _fillIcon;

        private Tweener _shakeTween;
        private bool _isFill;

        public void SetFill(bool isFill)
        {
            if (isFill == _isFill) return;

            _isFill = isFill;
            Animate();
        }

        private void Animate()
        {
            if (!_isFill)
            {
                _shakeTween?.Kill();
                _shakeTween = _fillIcon.transform.DOShakePosition(
                    duration: 0.5f, 
                    strength: new Vector3(10f, 10f, 0f), // насколько сильно трясет (пиксели)
                    vibrato: 10, // сколько "колебаний" за всё время
                    randomness: 90f, // насколько случайные колебания
                    snapping: false, // стоит ли округлять значения
                    fadeOut: false // чтобы тряска постепенно затухала
                );
            }

            _fillIcon.transform.DOScale(_isFill ? 1f : 0f, 0.5f).SetDelay(!_isFill ? 0.2f : 0f);
        }
    }
}