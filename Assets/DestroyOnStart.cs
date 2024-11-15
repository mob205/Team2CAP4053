using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : MonoBehaviour
{
    private void Start()
    {
        GameStateManager.Instance.OnGameStart.AddListener(() => Destroy(gameObject));
    }  
}
