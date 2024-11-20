using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] private MenuNavigator _start;
    private MenuNavigator _current;

    private void Start()
    {
        _current = _start;
        _current.Select();
    }
    public void OnLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        _current.Deselect();
        if (_current.Left)
        {
            _current = _current.Left;
        }
        _current.Select();
    }
    public void OnRight(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        _current.Deselect();
        if (_current.Right)
        {
            _current = _current.Right;
        }
        _current.Select();
    }
    public void OnNext(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        _current.Click();

        _current.Deselect();
        if (_current.Next)
        {
            _current = _current.Next;
        }
        _current.Select();
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        _current.GoBack();

        _current.Deselect();
        if(_current.Back)
        {
            _current = _current.Back;
        }
        _current.Select();
    }
}
