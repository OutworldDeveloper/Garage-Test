using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class Player : MonoBehaviour
{

    public event Action<IInteractable> InteractionTargetChanged;
    public event Action InteractionTargetLost;

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _carryingHeavyItemSpeed = 1.3f;
    [SerializeField] private float _acceleration = 25f;
    [SerializeField] private float _mouseSensitivity = 1f;
    [SerializeField] private Transform _head;
    [SerializeField] private float _headRotationLimit = 70f;
    [SerializeField] private float _interactionDistance = 1f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Transform _itemAttachmentPoint;

    private CharacterController _controller;
    private Vector3 _velocityXZ;
    private float _velocityY;

    private float _cameraTargetRotX;
    private float _cameraTargetRotY;

    private IInteractable _targetInteractable;

    public Item HoldingItem { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        var input = GatherInput();
        UpdateMovement(input);
        UpdateRotation(input);
        UpdateInteractionTarget();
        UpdateInteraction(input);
    }

    public void PickUp(Item item)
    {
        if (HoldingItem != null) return;

        HoldingItem = item;
        HoldingItem.transform.SetParent(_itemAttachmentPoint, false);
        HoldingItem.transform.localPosition = HoldingItem.HoldingOffset;
        HoldingItem.transform.localRotation = HoldingItem.HoldingRotation;
        item.OnPickedUp();
    }

    public Item ReleaseItem()
    {
        if (HoldingItem == null) throw new InvalidOperationException("Cannot release item when not holding any");

        var item = HoldingItem;
        HoldingItem.transform.SetParent(null, false);
        HoldingItem = null;
        item.OnReleased();
        return item;
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

        input.WantsInteract = Input.GetKey(KeyCode.E);

        return input;
    }

    private void UpdateMovement(PlayerInput input)
    {
        Vector3 desiredVelocity = transform.TransformDirection(input.MoveDirection) * GetCurrentSpeed();

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

    private float GetCurrentSpeed()
    {
        if (HoldingItem != null && HoldingItem.IsHeavy)
            return _carryingHeavyItemSpeed;

        return _speed;
    }

    private void UpdateRotation(PlayerInput input)
    {
        _cameraTargetRotY += input.Mouse.x;
        _cameraTargetRotX -= input.Mouse.y;

        _cameraTargetRotX = Mathf.Clamp(_cameraTargetRotX, -_headRotationLimit, _headRotationLimit);

        transform.eulerAngles = new Vector3(0f, _cameraTargetRotY, 0f);
        _head.localEulerAngles = new Vector3(_cameraTargetRotX, 0f, 0f);
    }

    private void UpdateInteractionTarget()
    {
        Ray ray = new Ray(_head.transform.position, _head.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _interactableLayer))
        {
            ClearTargetInteractable();
            return;
        }

        if (!hit.transform.TryGetComponent(out IInteractable interactable))
        {
            ClearTargetInteractable();
            return;
        }

        if (interactable == _targetInteractable) return;

        _targetInteractable = interactable;
        InteractionTargetChanged(interactable);
    }

    private void ClearTargetInteractable()
    {
        bool wasNull = _targetInteractable == null; 
        _targetInteractable = null;
        if (wasNull == false)
            InteractionTargetLost?.Invoke();
    }

    private void UpdateInteraction(PlayerInput input)
    {
        if (_targetInteractable == null) return;
        if (!input.WantsInteract) return;
        if (!_targetInteractable.IsAvaliable(this)) return;

        _targetInteractable.Interact(this);
        InteractionTargetChanged?.Invoke(_targetInteractable);
    }

    private struct PlayerInput
    {
        public Vector3 MoveDirection;
        public Vector2 Mouse;
        public bool WantsInteract;

    }

}
