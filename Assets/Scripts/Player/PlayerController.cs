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
    public void OnMove(InputAction.CallbackContext context)
    {
        // Player shouldn't move if the interacting with an interactable
        if(!_allowInput || (_interactor.CurrentInteractable != null))
        {
            MoveInput = Vector2.zero;
            _movement.InputMove(MoveInput);
            return; 
        }
        MoveInput = context.ReadValue<Vector2>();
        _movement.InputMove(MoveInput);
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
            _interactor.UnequipTool();
        }
    }
}
