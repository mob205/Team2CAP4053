using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ToolType RequiredTool { get; private set; } = null;
    [field: SerializeField] public ToolType ToolType { get; private set; }

    [field: SerializeField] public int InteractionPriority { get; private set; } = 1;

    private bool _isHeld;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void StartInteract(PlayerInteractor player)
    {
        _isHeld = player.EquipTool(this);
        if (_isHeld)
        {
            _collider.enabled = false;
        }
    }

    public void StopInteract(PlayerInteractor player)
    {
        // Do nothing, since the tool should be held even when the player released Interact input
    }
    public void DropTool()
    {
        _isHeld = false;
        _collider.enabled = true;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo);
        transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        transform.rotation = Quaternion.identity;
    }
    public bool IsInteractable(ToolType tool)
    {
        return !_isHeld && tool == null;
    }
}
