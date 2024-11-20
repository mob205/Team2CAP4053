using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] private Selectable _start;
    private Selectable _current;

    private void Start()
    {
        _current = _start;
    }
    public void OnLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (_current.FindSelectableOnLeft())
        {
            _current = _current.FindSelectableOnLeft();
        }
    }
    public void OnRight(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (_current.FindSelectableOnRight())
        {
            _current = _current.FindSelectableOnRight();
        }
    }
    public void OnNext(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if (_current.FindSelectableOnUp())
        {
            _current = _current.FindSelectableOnUp();
        }
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        if(_current.FindSelectableOnDown())
        {
            _current = _current.FindSelectableOnDown();
        }
    }
}
