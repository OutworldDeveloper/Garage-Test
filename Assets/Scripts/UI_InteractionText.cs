using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(2)]
public sealed class UI_InteractionText : MonoBehaviour
{

    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _interactionLabel;
    [SerializeField] private Image _crosshair;
    [SerializeField] private Color _crosshairInteractionColor;

    private void OnEnable()
    {
        _player.InteractionTargetChanged += OnInteractionTargetChanged;
        _player.InteractionTargetLost += OnInteractionTargetLost;
    }

    private void OnDisable()
    {
        _player.InteractionTargetChanged -= OnInteractionTargetChanged;
        _player.InteractionTargetLost -= OnInteractionTargetLost;
    }

    private void OnInteractionTargetLost()
    {
        _crosshair.color = Color.white;
        _interactionLabel.gameObject.SetActive(false);
    }

    private void OnInteractionTargetChanged(IInteractable interactable)
    {
        _crosshair.color = _crosshairInteractionColor;
        _interactionLabel.gameObject.SetActive(true);
        _interactionLabel.text = interactable.GetInteractionText();
    }
}
