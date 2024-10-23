using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Tool HeldTool { get; private set; }
    public IInteractable CurrentInteractable { get; private set; }

    [SerializeField] private Transform _toolPos;

    private Dictionary<IInteractable, Transform> _nearby = new Dictionary<IInteractable, Transform>();

    public bool EquipTool(Tool tool)
    {
        if(tool == null || HeldTool != null) { return false; }

        HeldTool = tool;
        HeldTool.transform.parent = _toolPos;
        HeldTool.transform.localPosition = Vector3.zero;
        HeldTool.transform.rotation = _toolPos.rotation;
        return true;
    }
    public void UnequipTool()
    {
        if (HeldTool == null) { return; }
        StopInteract();

        HeldTool.transform.parent = null;
        HeldTool.DropTool();
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
        var interactable = other.GetComponent<IInteractable>();
        if(interactable != null && !_nearby.ContainsKey(interactable))
        {
            _nearby.Add(interactable, other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if(interactable != null)
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
