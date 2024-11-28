using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    private void Start()
    {
        LevelManager.LoadLevel("Menu", false);
    }
}
