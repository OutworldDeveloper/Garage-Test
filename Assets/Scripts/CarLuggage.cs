using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CarLuggage : MonoBehaviour, IInteractable
{

    public event Action<Item> ItemPlaced;

    [SerializeField] private List<Transform> _spots;
    [SerializeField] private int _requiredItems = 9;

    public int ItemsCount { get; private set; }
    public int RequiredItems => _requiredItems;

    public string GetInteractionText()
    {
        return "Place item";
    }
    public bool IsAvaliable(Player player)
    {
        return player.HoldingItem != null;
    }

    public void Interact(Player player)
    {
        var item = player.ReleaseItem();
        item.OnPickedUp();

        int spotIndex = Mathf.Min(_spots.Count - 1, ItemsCount);
        item.transform.position = _spots[spotIndex].transform.position;
        item.transform.rotation = _spots[spotIndex].transform.rotation;
        ItemsCount++;
        ItemPlaced?.Invoke(item);
    }

}
