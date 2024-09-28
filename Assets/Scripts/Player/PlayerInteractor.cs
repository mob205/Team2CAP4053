using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Tool HeldTool { get; private set; }

    [SerializeField] private Transform _toolPos;

    private Dictionary<IInteractable, Transform> _nearby = new Dictionary<IInteractable, Transform>();
    private IInteractable _curInteractable;

    public bool EquipTool(Tool tool)
    {
        if(tool == null || HeldTool != null) { return false; }

        HeldTool = tool;
        HeldTool.transform.parent = _toolPos;
        HeldTool.transform.localPosition = Vector3.zero;
        HeldTool.transform.localRotation = _toolPos.localRotation;
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
        _curInteractable = GetClosestInteractable();
        _curInteractable?.StartInteract(this);
    }
    private IInteractable GetClosestInteractable()
    {
        IInteractable best = null;
        float closest = Mathf.Infinity;
        Vector3 cur = transform.position;
        foreach(var inter in _nearby)
        {
            if(!inter.Key.IsInteractable()) { continue; }
            float dist = (inter.Value.position - cur).sqrMagnitude;
            if(dist < closest)
            {
                closest = dist;
                best = inter.Key;
            }
        }
        return best;
    }
    public void StopInteract()
    {
        _curInteractable?.StopInteract(this);
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
            interactable.StopInteract(this);
            _nearby.Remove(interactable);
        }
    }
}
