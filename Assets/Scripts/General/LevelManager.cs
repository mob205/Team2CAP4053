using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class LevelInfo
{
    public float ClearScore;
    public string NextLevel;
    public string PreviousLevel;
    public bool IsDefaultUnlocked;

    public LevelInfo(float clearScore, float highscore, string nextLevel, string previousLevel, bool isDefaultUnlocked)
    {
        ClearScore = clearScore;
        NextLevel = nextLevel;
        PreviousLevel = previousLevel;
        IsDefaultUnlocked = isDefaultUnlocked;
    }
}

public static class LevelManager
{
    public static string LoadingLevel { get; private set; } = "Menu";

    private static Dictionary<string, LevelInfo> _levels = new Dictionary<string, LevelInfo>();

    private static string _loadingScene = "LoadingScreen";
    private static AsyncOperation _changeToLoadingScreen;
    private static AsyncOperation _changeToLevel;

    public static bool TrySetHighscore(string level, float score)
    {
        float curHighScore = PlayerPrefs.GetFloat(level, 0);
        if (curHighScore < score)
        {
            PlayerPrefs.SetFloat(level, score);
            return true;
        }
        return false;
    }
    public static float GetHighscore(string level)
    {
        return PlayerPrefs.GetFloat(level, 0);
    }

    public static string GetNextLevel(string level)
    {
        if (_levels.ContainsKey(level))
        {
            return _levels[level].NextLevel;
        }
        return null;
    }

    public static string GetPreviousLevel(string level)
    {
        if(_levels.ContainsKey(level))
        {
            return _levels[level].PreviousLevel;
        }
        return null;
    }

    public static int GetClearScore(string level)
    {
        if (_levels.ContainsKey(level))
        {
            return (int)_levels[level].ClearScore;
        }
        return 0;
    }

    public static bool IsLevelUnlocked(string level)
    {
        if (level == null || !_levels.ContainsKey(level)) { return false; }
        if (_levels[level].PreviousLevel == null || _levels[level].IsDefaultUnlocked) { return true; }

        var last = _levels[level].PreviousLevel;

        return GetHighscore(last) >= _levels[level].ClearScore;
    }

    public static void InitializeLevels(LevelLock[] levelLocks)
    {
        if (_levels.Count > 0) { return; }

        _levels = new Dictionary<string, LevelInfo>();

        string lastLevel = null;
        foreach (var levelLock in levelLocks)
        {
            LevelInfo cur = new LevelInfo(levelLock.ClearScore, GetHighscore(levelLock.Level), null, lastLevel == null ? null : string.Copy(lastLevel), levelLock.IsDefaultUnlocked);

            if (lastLevel != null)
            {
                _levels[lastLevel].NextLevel = levelLock.Level;
            }

            lastLevel = levelLock.Level;
            _levels.Add(levelLock.Level, cur);
        }

    }

    public static void LoadLevel(string level)
    {
        if(_changeToLevel != null) { Debug.Log(LoadingLevel); return; }

        _changeToLevel = SceneManager.LoadSceneAsync(level);
        _changeToLevel.completed += (_) => PreloadLoadingScreen();

        if (_changeToLoadingScreen != null)
        {
            _changeToLevel.allowSceneActivation = false;

            _changeToLoadingScreen.allowSceneActivation = true;
            _changeToLoadingScreen = null;
        }

        LoadingLevel = level;
    }
    public static void FinishLoading()
    {
        if(_changeToLevel != null)
        {
            _changeToLevel.allowSceneActivation = true;
            _changeToLevel = null;
        }
        LoadingLevel = null;
    }
    public static float GetLoadingProgress()
    {
        if(_changeToLevel != null)
        {
            return _changeToLevel.progress;
        }
        return 0;
    }
    public static void PreloadLoadingScreen()
    {
        if(_changeToLoadingScreen == null)
        {
            _changeToLoadingScreen = SceneManager.LoadSceneAsync(_loadingScene);
            _changeToLoadingScreen.allowSceneActivation = false;
        }
    }
}
