using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnReady : MonoBehaviour
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
        ReadyUpManager.OnReadyUp += EnableAll;
    }
    private void EnableAll()
    {
        foreach(var item in _enableOnReady)
        {
            item.SetActive(true);
        }
    }
}
