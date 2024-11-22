using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameLoseUI : MonoBehaviour
{
    [SerializeField] private InputActionProperty _nextAction;
    [SerializeField] private InputActionProperty _quitAction;

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _menuButton;

    private void OnEnable()
    {
        foreach (var player in GameStateManager.Instance.Players)
        {
            player.actions[_nextAction.reference.name].started += (InputAction.CallbackContext context) => _nextButton.onClick.Invoke();

            player.actions[_quitAction.reference.name].started += (InputAction.CallbackContext context) => _menuButton.onClick.Invoke();
        }
    }
}
