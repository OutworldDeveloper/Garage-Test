using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CarLuggage : MonoBehaviour, IInteractable
{

    public event Action<Item> ItemPlaced;

    [SerializeField] private Transform[] _points;

    public int ItemsCount { get; private set; }

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
        item.transform.position = _points[ItemsCount].transform.position;
        item.transform.rotation = _points[ItemsCount].transform.rotation;
        ItemsCount++;
        ItemPlaced?.Invoke(item);
    }

}
