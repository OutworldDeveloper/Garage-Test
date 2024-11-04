using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(2)]
public sealed class UI_Finish : MonoBehaviour
{

    [SerializeField] private CarLuggage _luggage;
    [SerializeField] private TextMeshProUGUI _finishLabel;

    private void OnEnable()
    {
        RefreshFinishLabel();
        _luggage.ItemPlaced += OnItemPlaced;
    }

    private void OnDisable()
    {
        _luggage.ItemPlaced -= OnItemPlaced;
    }

    private void OnItemPlaced(Item item)
    {
        RefreshFinishLabel();
    }

    private void RefreshFinishLabel()
    {
        _finishLabel.gameObject.SetActive(_luggage.ItemsCount == _luggage.RequiredItems);
    }

}
