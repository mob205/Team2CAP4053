using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OracleInteractable : DurationInteractable
{
    [SerializeField] private float _interactionDuration;

    private OracleAI _oracle;
    private void Awake()
    {
        _oracle = GetComponent<OracleAI>();
    }

    protected override void CompleteInteraction()
    {
        _oracle.Kill();
    }

    protected override float GetInteractionDuration()
    {
        return _interactionDuration;
    }
}
