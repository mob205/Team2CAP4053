using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Tool HeldTool { get; private set; }
    public IInteractable CurrentInteractable { get; private set; }
    [field: SerializeField] public Transform ToolPos { get; private set; }

    private Dictionary<IInteractable, Transform> _nearby = new Dictionary<IInteractable, Transform>();

    public bool EquipTool(Tool tool)
    {
        if(tool == null || HeldTool != null) { return false; }

        HeldTool = tool;
        return true;
    }
    public void UnequipTool()
    {
        if (HeldTool == null) { return; }
        StopInteract();

        HeldTool.OnDropTool();
        HeldTool = null;
    }
    public void StartInteract()
    {
        CurrentInteractable = GetClosestInteractable();
        CurrentInteractable?.StartInteract(this);
    }

    // Gets the closest interactable with the highest priority
    private IInteractable GetClosestInteractable()
    {
        IInteractable best = null;
        float closest = Mathf.Infinity;
        int bestPriority = int.MinValue;
        Vector3 cur = transform.position;

        foreach(var inter in _nearby)
        {
            if(!inter.Value || !inter.Key.IsInteractable(GetHeldToolType())) { continue; }  // Only consider interactable if it can be interacted with

            float dist = (inter.Value.position - cur).sqrMagnitude;
            if(bestPriority < inter.Key.InteractionPriority || dist < closest)
            {
                closest = dist;
                best = inter.Key;
                bestPriority = inter.Key.InteractionPriority;
            }
        }
        return best;
    }
    public void StopInteract()
    {
        CurrentInteractable?.StopInteract(this);
        CurrentInteractable = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interactable) && !_nearby.ContainsKey(interactable))
        {
            _nearby.Add(interactable, other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<IInteractable>(out var interactable))
        {
            _nearby.Remove(interactable);
            
            if(interactable == CurrentInteractable)
            {
                interactable.StopInteract(this);
            }
        }
    }
    public ToolType GetHeldToolType()
    {
        if(HeldTool)
        {
            return HeldTool.ToolType;
        }
        else
        {
            return null;
        }
    }
}
