using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ToolType RequiredTool { get; private set; }

    [field: SerializeField] public int InteractionPriority { get; private set; } = 1;

    public UnityEvent OnInteractionFinished { get; }

    private Enemy _parent;
    private void Awake()
    {
        if(!transform.parent.TryGetComponent(out _parent))
        {
            Debug.LogError("EnemyInteractable found on an object that is not an Enemy.");
            Destroy(gameObject);
        }
    }
    public bool IsInteractable(ToolType tool)
    {
        return tool == RequiredTool;
    }

    public void StartInteract(PlayerInteractor player)
    {
        _parent.Kill();
        player.StopInteract();
    }

    public void StopInteract(PlayerInteractor player)
    {
    }
}
