using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DurationInteractable : MonoBehaviour, IInteractable
{
    [field: Header("General")]
    [field: SerializeField] public ToolType RequiredTool { get; private set; }

    [field: SerializeField] public int InteractionPriority { get; private set; } = 0;

    [field: SerializeField] public int NumInteractorsRequired { get; private set; } = 1;

    public int CurrentNumInteractors { get { return _interactors.Count; } }

    public float MaxDuration { get { return GetInteractionDuration(); } }

    public float TimeRemaining { get; private set; }
    public bool IsInProgress { get; private set; }

    private List<PlayerInteractor> _interactors = new List<PlayerInteractor>();

    public virtual bool IsInteractable(ToolType tool)
    {
        return _interactors.Count < NumInteractorsRequired && (RequiredTool == null || tool == RequiredTool);
    }

    public virtual void StartInteract(PlayerInteractor player)
    {
        _interactors.Add(player);

        if(_interactors.Count == NumInteractorsRequired)
        {
            TimeRemaining = MaxDuration;
            IsInProgress = true;
        }
    }

    public virtual void StopInteract(PlayerInteractor player)
    {
        _interactors.Remove(player);
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
