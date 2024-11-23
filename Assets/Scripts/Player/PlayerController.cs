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

    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _interactor = GetComponent<PlayerInteractor>(); 
        _playerID = nextID++;
    }
    private void Update()
    {
        if(!_allowInput || _interactor && _interactor.CurrentInteractable != null)
        {
            _movement.InputMove(Vector2.zero);
        }
        else
        {
            _movement.InputMove(MoveInput);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
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
