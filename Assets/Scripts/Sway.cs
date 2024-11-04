using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Sway : MonoBehaviour
{

    [SerializeField] private float _speed = 50f;

    private Quaternion _currentRotation;

    private void Start()
    {
        _currentRotation = transform.parent.rotation;
    }

    private void LateUpdate()
    {
        var angle = Quaternion.Angle(_currentRotation, transform.parent.rotation);
        var t = Mathf.Clamp01(angle / 30f);
        var z = Mathf.Lerp(0f, 5f, t);

        _currentRotation.eulerAngles = new Vector3(_currentRotation.eulerAngles.x, _currentRotation.eulerAngles.y, z);

        _currentRotation = Quaternion.Slerp(_currentRotation, transform.parent.rotation, Time.deltaTime * _speed);
        transform.rotation = _currentRotation;
    }

}
