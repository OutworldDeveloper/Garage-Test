using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{

    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public bool IsHeavy { get; private set; }
    [field: SerializeField] public Vector3 HoldingOffset { get; private set; }
    [field: SerializeField] public Quaternion HoldingRotation { get; private set; }

    private Collider[] _colliders;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>();
    }

    public string GetInteractionText()
    {
        return $"Pick up {DisplayName}";
    }

    public bool IsAvaliable(Player player)
    {
        return player.HoldingItem == null;
    }

    public void Interact(Player player)
    {
        player.PickUp(this);
    }

    public void OnPickedUp()
    {
        EnableColliders(false);
    }

    public void OnReleased()
    {
        EnableColliders(true);
    }

    private void EnableColliders(bool enable)
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = enable;
        }
    }

}
