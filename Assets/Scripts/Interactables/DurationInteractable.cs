using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    public UnityEvent OnInteractionFinished { get; }

    private List<PlayerInteractor> _interactors = new List<PlayerInteractor>();

    protected virtual void Start()
    {
        if(GameStateManager.Instance.IsGameActive)
        {
            UpdateNumInteractors();
        }
        else
        {
            GameStateManager.Instance.OnGameStart.AddListener(UpdateNumInteractors);
        }
    }

    private void UpdateNumInteractors()
    {
        NumInteractorsRequired = Mathf.Min(NumInteractorsRequired, GameStateManager.Instance.Players.Count);
    }
    public virtual bool IsInteractable(ToolType tool)
    {
        return _interactors.Count < NumInteractorsRequired && (RequiredTool == null || tool == RequiredTool);
    }

    public virtual void StartInteract(PlayerInteractor player)
    {
        _interactors.Add(player);

        var numInteractors = Mathf.Min(NumInteractorsRequired, GameStateManager.Instance.Players.Count);

        if(_interactors.Count == numInteractors)
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
            
            for(int i = _interactors.Count - 1; i >= 0; i--)
            {
                _interactors[i].StopInteract();
            }
        }
    }

    protected abstract void CompleteInteraction();
    protected abstract float GetInteractionDuration();
}
