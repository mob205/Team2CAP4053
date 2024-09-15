using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInteractor : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ToolType RequiredTool { get; private set; }

    [SerializeField] private float _passiveChangeSpeed = 5f;
    [SerializeField] private float _interactingChangeSpeed = 25f; 

    private bool _isInteracting;
    private PlayerInteractor _interactor;
    private float _value;
    private Material _material;

    private void Start()
    {
       _material = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        if(_isInteracting)
        {
            _value -= (_interactingChangeSpeed / 100) * Time.deltaTime;
        }
        else
        {
            _value += (_passiveChangeSpeed / 100) * Time.deltaTime;
        }
        _value = Mathf.Clamp(_value, 0, 1);
        _material.color = new Color(1 - _value, 1, 1 - _value);

    }

    public void StartInteract(PlayerInteractor player)
    {
        Debug.Log("Interacting!");
        if(_interactor == null && HasCorrectTool(player))
        {
            _interactor = player;
            _isInteracting = true;
        }
    }

    public void StopInteract(PlayerInteractor player)
    {
        if(player == _interactor)
        {
            _isInteracting = false;
            _interactor = null;
        }
    }

    private bool HasCorrectTool(PlayerInteractor player)
    {
        Debug.Log("Checking for correct tool.");
        return (player.HeldTool == null && RequiredTool == null) || (player.HeldTool != null && player.HeldTool.ToolType == RequiredTool);
    }

    public bool IsInteractable()
    {
        return true;
    }
}
