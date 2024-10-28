using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DurationInteractable : MonoBehaviour, IInteractable
{
    [field: Header("General")]
    [field: SerializeField] public ToolType RequiredTool { get; private set; }

    [field: SerializeField] public int InteractionPriority { get; private set; } = 0;

    public float MaxDuration { get { return GetInteractionDuration(); } }

    public float TimeRemaining { get; private set; }
    public bool IsInProgress { get; private set; }

    private PlayerInteractor _interactor;

    public virtual bool IsInteractable(ToolType tool)
    {
        return _interactor == null && (RequiredTool == null || tool == RequiredTool);
    }

    public virtual void StartInteract(PlayerInteractor player)
    {

        _interactor = player;
        TimeRemaining = MaxDuration;
        IsInProgress = true;
    }

    public virtual void StopInteract(PlayerInteractor player)
    {
        _interactor = null;
        IsInProgress = false;
    }

    protected virtual void Update()
    {
        if(!IsInProgress) { return; }
        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0)
        {
            IsInProgress = false;
            CompleteInteraction();
        }
    }

    protected abstract void CompleteInteraction();
    protected abstract float GetInteractionDuration();
}
