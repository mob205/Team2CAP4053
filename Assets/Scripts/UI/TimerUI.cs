using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private GameStateManager _stateManager;
    private TextMeshProUGUI _textUI;

    private void Start()
    {
        _stateManager = FindObjectOfType<GameStateManager>();
        if(_stateManager == null)
        {
            Debug.Log("No state manager found.");
            Destroy(this);
        }
        _textUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _textUI.text = "Time Left: " + Mathf.CeilToInt(_stateManager.TimeRemaining).ToString();
    }
}
