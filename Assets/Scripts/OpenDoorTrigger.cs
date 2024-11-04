using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorTrigger : MonoBehaviour
{

    [SerializeField] private Door _target;

    private void OnTriggerEnter(Collider other)
    {
        if (_target.IsOpen) return;
        if (other.GetComponent<Player>() == null) return;

        _target.Open();
    }

}
