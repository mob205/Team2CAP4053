using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUI : MonoBehaviour
{
    [SerializeField] private PlayerReadyDisplay[] _readyDisplays;
    private void Start()
    {
        ReadyUpManager.Instance.OnReadyStateChange.AddListener(UpdateUI);
        ReadyUpManager.Instance.OnReadyUp.AddListener(DestroyUI);
        UpdateUI();
    }
    private void UpdateUI()
    {
        foreach(var display in _readyDisplays)
        {
            display.gameObject.SetActive(false);
        }

        var states = ReadyUpManager.Instance.ReadyStates;
        for(int i = 0; i < states.Count; i++)
        {
            _readyDisplays[i].gameObject.SetActive(true);
            _readyDisplays[i].SetReadyState(states[i].Value);
        }
    }
    private void DestroyUI()
    {
        Destroy(gameObject);
    }
}
