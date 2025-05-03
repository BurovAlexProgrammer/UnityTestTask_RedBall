using DG.Tweening;
using UnityEngine;

namespace Common.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AutoTransparent : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<SpriteRenderer>().DOFade(0f, 0f);
        }
    }
}