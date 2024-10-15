using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInLevel : MonoBehaviour
{
    [Tooltip("Objects to enable when the Ready signal is given")]
    [SerializeField] private GameObject[] _enableOnReady;

    private MeshRenderer _test;

    private void Start()
    {
        foreach(var item in _enableOnReady)
        {
            item.SetActive(false);
        }
        ReadyUpManager.Instance.OnReadyUp.AddListener(EnableAll);
        GameStateManager.Instance.OnGameEnd.AddListener(DisableAll);
    }
    private void EnableAll()
    {
        foreach(var item in _enableOnReady)
        {
            item.SetActive(true);
        }
    }
    private void DisableAll()
    {
        foreach(var item in _enableOnReady)
        {
            item.SetActive(false);
        }
    }
}
