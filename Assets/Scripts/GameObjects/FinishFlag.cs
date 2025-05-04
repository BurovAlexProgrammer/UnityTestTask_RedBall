using UnityEngine;

namespace GameObjects
{
    [RequireComponent(typeof(Collider2D))]
    public class FinishFlag : MonoBehaviour
    {
        private Collider2D _collider2D;

        private void OnValidate()
        {
            gameObject.tag = "Finish";

            _collider2D = GetComponent<Collider2D>();
            _collider2D.isTrigger = true;
        }
    }
}