using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    [Tooltip("The type of tool that the interacting player must have to use this interactable item")]
    public ToolType RequiredTool { get; }
    public void StartInteract(PlayerInteractor player);
    public void StopInteract(PlayerInteractor player);

    public bool IsInteractable();
}
