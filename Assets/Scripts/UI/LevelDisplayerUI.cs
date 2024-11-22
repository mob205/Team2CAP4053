using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelDisplayerUI : MonoBehaviour
{
    [SerializeField] private string _level;
    [SerializeField] private TextMeshProUGUI _text;
    private void Start()
    {
        if(LevelManager.IsLevelUnlocked(_level))
        {
            _text.text = "High:\n" + (int)LevelManager.GetHighscore(_level);
        }
        else
        {
            _text.text = "LOCKED!\nRequires score of\n" + LevelManager.GetClearScore(_level) + "\n on previous level to unlock.";
        }
    }
}
