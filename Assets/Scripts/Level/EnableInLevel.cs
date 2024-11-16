using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInLevel : MonoBehaviour
{
    private MeshRenderer _test;

    private DurationInteractable[] _enableOnReady;

    private void Awake()
    {
        _enableOnReady = GetComponentsInChildren<DurationInteractable>();
    }

    private void Start()
    {
        foreach(var item in _enableOnReady)
        {
            item.enabled = false;
        }
        ReadyUpManager.Instance.OnReadyUp.AddListener(EnableAll);
        GameStateManager.Instance.OnGameEnd.AddListener(DisableAll);
    }
    private void EnableAll()
    {
        foreach(var item in _enableOnReady)
        {
            item.enabled = true;
        }
    }
    private void DisableAll()
    {
        foreach(var item in _enableOnReady)
        {
            item.enabled = false;
        }
    }
}
