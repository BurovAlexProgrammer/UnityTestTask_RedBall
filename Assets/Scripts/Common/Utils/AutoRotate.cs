using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private float _speed = 360f;
    
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        _transform.Rotate(Vector3.forward, _speed * Time.deltaTime);
    }
}
