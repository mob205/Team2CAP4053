using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour
{

    [SerializeField] private InputActionProperty _nextAction;
    [SerializeField] private InputActionProperty _quitAction;

    [SerializeField] private string _nextScene;
    [SerializeField] private string _menuScene;
    [SerializeField] private string _loadingScene;

    private void OnEnable()
    {
        foreach(var player in GameStateManager.Instance.Players)
        {
            player.actions[_nextAction.reference.name].started += (InputAction.CallbackContext context) => ChangeSceneToNextLevel();

            player.actions[_quitAction.reference.name].started += (InputAction.CallbackContext context) => ChangeSceneToMenu();
        }
    }

    public void ChangeSceneToNextLevel()
    {
        SavedData.NextScene = _nextScene;
        StartChangeScene();
    }
    public void ChangeSceneToMenu()
    {
        SavedData.NextScene = _menuScene;
        StartChangeScene();
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(_loadingScene);
    }

    private void StartChangeScene()
    {
        foreach (var player in GameStateManager.Instance.Players)
        {
            player.actions[_nextAction.action.id.ToString()].started -= (InputAction.CallbackContext context) => ChangeSceneToNextLevel();
            player.actions[_nextAction.action.id.ToString()].started -= (InputAction.CallbackContext context) => ChangeSceneToMenu();
        }
        if(this)
        {
            Invoke(nameof(ChangeScene), 0);
        }
    }
}
