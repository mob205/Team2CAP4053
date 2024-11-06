using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ToolType RequiredTool { get; private set; } = null;
    [field: SerializeField] public ToolType ToolType { get; private set; }

    [field: SerializeField] public int InteractionPriority { get; private set; } = 1;

    private Collider _collider;

    protected bool _isHeld;
    protected PlayerInteractor _heldPlayer;

    protected virtual void Update()
    {
        if(_isHeld && _heldPlayer)
        {
            transform.SetPositionAndRotation(_heldPlayer.ToolPos.position, _heldPlayer.ToolPos.rotation);
        }
    }

    public virtual void StartInteract(PlayerInteractor player)
    {
        _isHeld = player.EquipTool(this);
        if (_isHeld)
        {
            _heldPlayer = player;
        }
    }

    public virtual void StopInteract(PlayerInteractor player)
    {
        // Do nothing, since the tool should be held even when the player released Interact input
    }
    public virtual void OnDropTool()
    {
        _isHeld = false;
        _heldPlayer = null;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo);

        transform.SetPositionAndRotation(
            new Vector3(transform.position.x, hitInfo.point.y, transform.position.z), 
            Quaternion.identity
            );
    }
    public bool IsInteractable(ToolType tool)
    {
        return !_isHeld && tool == null;
    }
}
