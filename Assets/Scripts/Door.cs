using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{

    private const string ANIMATOR_OPEN_KEY = "Open";

    private Animator _animator;
    public bool IsOpen { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Open()
    {
        if (IsOpen) return;
        _animator.SetBool(ANIMATOR_OPEN_KEY, true);
        IsOpen = true;
    }

}
