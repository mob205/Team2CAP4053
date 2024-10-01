using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReviver : IInteractable
{
    [field: SerializeField] public ToolType RequiredTool { get; private set; }

    [Tooltip("A shared SO which represents the number of times a player has been revived this round")]
    [SerializeField] ScriptableInt _revivesUsed;

    [Tooltip("The amount of time, in seconds, it should take to perform a revive depending on how many revives were previously performed.")]
    [SerializeField] private float[] _reviveDurations;

    private float _curReviveTimer;
    private bool _isReviving;

    private PlayerInteractor _interactor;

    public bool IsInteractable(ToolType tool)
    {
        return _interactor == null && tool == RequiredTool;
    }

    public void StartInteract(PlayerInteractor player)
    {
        if(_interactor == null)
        {
            _curReviveTimer = _reviveDurations[Mathf.Min(_reviveDurations.Length - 1, _revivesUsed.Value)];
            _isReviving = true;
            _interactor = player;
        }
    }

    public void StopInteract(PlayerInteractor player)
    {
        if(_interactor == player)
        {
            _isReviving = false;
            _interactor = null;
        }
    }

    private void Update()
    {

    }
}
