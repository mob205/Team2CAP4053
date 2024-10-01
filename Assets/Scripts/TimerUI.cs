using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private GameStateManager _stateManager;

    private TextMeshProUGUI _textUI;

    private void Start()
    {
        if(_stateManager == null)
        {
            Debug.Log("No state manager provided to timerUI.");
            Destroy(this);
        }
        _textUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _textUI.text = ((int)_stateManager.TimeRemaining).ToString();
    }
}
