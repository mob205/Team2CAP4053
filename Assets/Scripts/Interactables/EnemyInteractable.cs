using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ToolType RequiredTool { get; private set; }

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
    }

    public void StopInteract(PlayerInteractor player)
    {
    }
}
