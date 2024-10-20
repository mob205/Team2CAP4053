using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ToolType RequiredTool { get; private set; }
    public bool IsInteractable(ToolType tool)
    {
        return tool == RequiredTool;
    }

    public void StartInteract(PlayerInteractor player)
    {
        // Some death effect here
        Destroy(transform.parent.gameObject);
    }

    public void StopInteract(PlayerInteractor player)
    {
    }
}
