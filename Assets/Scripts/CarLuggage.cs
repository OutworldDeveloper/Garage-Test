using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CarLuggage : MonoBehaviour, IInteractable
{

    public event Action<Item> ItemPlaced;

    [SerializeField] private List<Transform> _points;

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

        int pointIndex = Mathf.Min(_points.Count - 1, ItemsCount);
        item.transform.position = _points[pointIndex].transform.position;
        item.transform.rotation = _points[pointIndex].transform.rotation;
        ItemsCount++;
        ItemPlaced?.Invoke(item);
    }

}
