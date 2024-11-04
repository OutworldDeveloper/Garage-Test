using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DummyInteractable : MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "Interact";
    }

    public void Interact(Player player)
    {
        Debug.Log("Interacted with dummy interactable!");
    }

}
