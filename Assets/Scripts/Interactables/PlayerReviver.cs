using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerReviver : DurationInteractable
{
    [Tooltip("A shared SO which represents the number of times a player has been revived this round")]
    [SerializeField] ScriptableInt _revivesUsed;

    [Tooltip("The amount of time, in seconds, it should take to perform a revive depending on how many revives were previously performed.")]
    [SerializeField] private float[] _reviveDurations;

    [Tooltip("The PlayerHealth component of the attached player")]
    [SerializeField] private PlayerHealth _playerHealth;

    protected override void Start()
    {
        base.Start();

        if(_playerHealth == null)
        {
            Debug.LogError($"No player health found on player {gameObject.name}. Deleting this PlayerReviver");
            Destroy(this);
        }
    }

    public override bool IsInteractable(ToolType tool)
    {
        return _playerHealth.IsDead && base.IsInteractable(tool);
    }

    protected override void CompleteInteraction()
    {
        ++_revivesUsed.Value;
        _playerHealth.Revive();
    }

    protected override float GetInteractionDuration()
    {
        return _reviveDurations[Mathf.Min(_reviveDurations.Length - 1, _revivesUsed.Value)];
    }
}
