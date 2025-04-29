using System;
using Common;
using UnityEngine;

namespace GameObjects
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageCollider : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;

        private void OnValidate()
        {
            _collider ??= GetComponent<Collider2D>();

            if (_collider != null && _collider.isTrigger)
            {
                Debug.LogError("DamageCollider: Collider must be not a trigger");
            }


        }

        private void Start()
        {
            if (gameObject.layer != LayerMasks.Damage)
            {
                Debug.LogWarning($"DamageCollider: LayerMask of gameObject [{gameObject.name}] changed to 'Damage'");
                gameObject.layer = LayerMasks.Damage;
            } 
        }
    }
}