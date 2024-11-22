using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLevel(string level)
    {
        LevelManager.LoadLevel(level);
    }
    public void ReloadLevel()
    {
        LevelManager.LoadLevel(SceneManager.GetActiveScene().name);
    }
    public void LoadNextLevel()
    {
        LevelManager.LoadLevel(LevelManager.GetNextLevel(SceneManager.GetActiveScene().name));
    }
}
