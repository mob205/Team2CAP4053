using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAllLevels : MonoBehaviour
{
    public void UnlockLevels()
    {
        LevelManager.SetForcedLocks(true);
        LevelManager.LoadLevel("Menu", false);
    }
}
