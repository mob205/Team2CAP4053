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

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _interactor = GetComponent<PlayerInteractor>(); 
        _playerID = nextID++;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _movement.InputMove(context.ReadValue<Vector2>());
    }
    public void OnUse(InputAction.CallbackContext context)
    {
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
        if(context.started)
        {
            _interactor.UnequipTool();
        }
    }
}