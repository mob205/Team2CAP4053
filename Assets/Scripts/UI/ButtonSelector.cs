using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] private MenuNavigator _start;
    [SerializeField] private float _lockoutTime;
    private MenuNavigator _current;

    private bool _canInput = true;

    private void Start()
    {
        _current = _start;
        _current.Select();
    }
    public void OnLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        _current.GoLeft();

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

        _current.GoRight();

        _current.Deselect();
        if (_current.Right)
        {
            _current = _current.Right;
        }
        _current.Select();
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        _current.GoUp();

        _current.Deselect();
        if (_current.Up)
        {
            _current = _current.Up;
        }
        _current.Select();
    }
    public void OnDown(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        _current.GoDown();

        _current.Deselect();
        if (_current.Down)
        {
            _current = _current.Down;
        }
        _current.Select();
    }
    public void OnNext(InputAction.CallbackContext context)
    {
        if (!context.performed || !_canInput) { return; }

        _current.Click();

        _current.Deselect();
        if (_current.Next)
        {
            _current = _current.Next;
            StartCoroutine(LockInput());
        }
        _current.Select();
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (!context.performed || !_canInput) { return; }

        _current.GoBack();

        _current.Deselect();
        if(_current.Back)
        {
            _current = _current.Back;
            StartCoroutine(LockInput());
        }
        _current.Select();
    }

    private IEnumerator LockInput()
    {
        _canInput = false;
        yield return new WaitForSeconds(_lockoutTime);
        _canInput = true;
    }
}
