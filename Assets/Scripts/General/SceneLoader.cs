using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLevel(string level)
    {
        LevelManager.LoadLevel(level, false);
    }
    public void LoadLevelWithCutscene(string level)
    {
        LevelManager.LoadLevel(level, true);
    }
    public void ReloadLevel()
    {
        LevelManager.LoadLevel(SceneManager.GetActiveScene().name, false);
    }
    public void LoadNextLevel()
    {
        LevelManager.LoadLevel(LevelManager.GetNextLevel(SceneManager.GetActiveScene().name), true);
    }
}
