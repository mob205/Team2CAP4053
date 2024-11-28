using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct LevelLock
{
    public string Level;
    public int ClearScore;
    public bool IsDefaultUnlocked;
}
public class MenuLevelLoader : MonoBehaviour
{
    [SerializeField] private LevelLock[] _levels;

    private void Awake()
    {
        LevelManager.InitializeLevels(_levels);   
    }
    private void Start()
    {
        LevelManager.PreloadLoadingScreen();
    }
    public void TryLoadLevel(string level)
    {
        if(LevelManager.IsLevelUnlocked(level))
        {
            LevelManager.LoadLevel(level, true);
        }
    }
}
