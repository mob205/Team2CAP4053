using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinUI : MonoBehaviour
{
    [SerializeField] private InputActionProperty _nextAction;
    [SerializeField] private InputActionProperty _quitAction;

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private TextMeshProUGUI _highscoreText;

    private bool _canGoNext;

    private void OnEnable()
    {
        if (LevelManager.IsLevelUnlocked(LevelManager.GetNextLevel(SceneManager.GetActiveScene().name)))
        {
            _canGoNext = true;
            _retryButton.gameObject.SetActive(false);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);
        }

        _highscoreText.gameObject.SetActive(GameStateManager.Instance.HasHighscore);

        foreach (var player in GameStateManager.Instance.Players)
        {
            player.actions[_nextAction.reference.name].started += (InputAction.CallbackContext context) =>
            {
                if (_canGoNext)
                {
                    _nextButton.onClick.Invoke();
                }
                else
                {
                    _retryButton.onClick.Invoke();
                }
            };

            player.actions[_quitAction.reference.name].started += (InputAction.CallbackContext context) => _menuButton.onClick.Invoke();
        }
    }
}
