using System;
using UnityEngine;
using Zenject;

namespace GameObjects
{
    public class RelativeBackground : MonoBehaviour
    {
        [Inject] private PlayerController _playerController;

        [SerializeField] private float _moveDevider;
        
        
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void LateUpdate()
        {
            var x = _playerController.Position.x / _moveDevider;
            _transform.position = new Vector3(x, _transform.position.y, _transform.position.z);
        }
    }
}