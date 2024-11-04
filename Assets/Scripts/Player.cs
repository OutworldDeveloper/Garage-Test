using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class Player : MonoBehaviour
{

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _acceleration = 25f;
    [SerializeField] private float _mouseSensitivity = 1f;
    [SerializeField] private Transform _head;
    [SerializeField] private float _headRotationLimit = 70f;

    private CharacterController _controller;
    private Vector3 _velocityXZ;
    private float _velocityY;

    private float _cameraTargetRotX;
    private float _cameraTargetRotY;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var input = GatherInput();
        UpdateMovement(input);
        UpdateRotation(input);
    }

    private PlayerInput GatherInput()
    {
        var input = new PlayerInput();

        input.Mouse = new Vector2()
        {
            x = Input.GetAxis("Mouse X") * _mouseSensitivity,
            y = Input.GetAxis("Mouse Y") * _mouseSensitivity
        };

        input.MoveDirection = new Vector3()
        {
            z = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0,
            x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0
        }.normalized;

        return input;
    }

    private void UpdateMovement(PlayerInput input)
    {
        Vector3 desiredVelocity = transform.TransformDirection(input.MoveDirection) * _speed;

        _velocityXZ = Vector3.MoveTowards(_velocityXZ, desiredVelocity, _acceleration * Time.deltaTime);

        if (_controller.isGrounded)
        {
            _velocityY = -9.8f;
        }
        else
        {
            _velocityY -= 9.8f * Time.deltaTime;
        }

        Vector3 finalMove = new Vector3()
        {
            x = _velocityXZ.x,
            y = _velocityY,
            z = _velocityXZ.z,
        };

        finalMove *= Time.deltaTime;

        _controller.Move(finalMove);
    }

    private void UpdateRotation(PlayerInput input)
    {
        _cameraTargetRotY += input.Mouse.x;
        _cameraTargetRotX -= input.Mouse.y;

        _cameraTargetRotX = Mathf.Clamp(_cameraTargetRotX, -_headRotationLimit, _headRotationLimit);

        transform.eulerAngles = new Vector3(0f, _cameraTargetRotY, 0f);
        _head.localEulerAngles = new Vector3(_cameraTargetRotX, 0f, 0f);
    }

    private struct PlayerInput 
    {
        public Vector3 MoveDirection;
        public Vector2 Mouse;
    }

}
