using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    private Movement _movement;
    private PlayerInteractor _interactor;

    // TODO: there's probably a better way to generate unique IDs, if they're even needed at all
    private static int nextID = 0;
    private int _playerID;
    private bool _allowInput = true;

    public Vector2 MoveVector
    {
        get
        {
            if (!_allowInput || (_interactor && _interactor.CurrentInteractable != null))
            {
                return Vector2.zero;
            }
            return _moveInput;
        }
    }

    private Vector2 _moveInput;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _interactor = GetComponent<PlayerInteractor>(); 
        _playerID = nextID++;
    }
    private void Update()
    {
        _movement.InputMove(MoveVector);
        //if(!_allowInput || _interactor && _interactor.CurrentInteractable != null)
        //{
        //    _movement.InputMove(Vector2.zero);
        //    Debug.Log("Movement locked.");
        //}
        //else
        //{
        //    Debug.Log("Movement freed.");
        //    _movement.InputMove(_moveInput);
        //}
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void OnUse(InputAction.CallbackContext context)
    {
        if(!_allowInput) { return; }
        if(context.started)
        {
            _interactor.StartInteract();
        }
        else if(context.canceled)
        {
            _interactor.StopInteract();
        }
    }
    public void OnDrop(InputAction.CallbackContext context)
    {
        if(!_allowInput) { return; }
        if(context.started)
        {
            _interactor.UnequipTool();
        }
    }
    public void ChangeInputAllow(bool allowed)
    {
        _allowInput = allowed;
        if(!_allowInput)
        {
            _movement.InputMove(Vector2.zero);
            _interactor.StopInteract();
            _interactor.UnequipTool();
        }
    }
}
