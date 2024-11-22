using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearScoreDisplay : MonoBehaviour
{
    private void Awake()
    {
        var text = GetComponent<TextMeshProUGUI>();
        text.text = LevelManager.GetClearScore(SceneManager.GetActiveScene().name).ToString();
    }
}
