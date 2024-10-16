using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{

    [SerializeField] private InputActionProperty _nextAction;
    [SerializeField] string _scene;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(_scene);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(_scene);
    }

    public void TestSubscribe()
    {
        transform.parent.gameObject.SetActive(true);
        foreach(var player in GameStateManager.Instance.Players)
        {
            player.actions[_nextAction.action.id.ToString()].started += ChangeSceneDelayed;
        }
    }

    private void ChangeSceneDelayed(InputAction.CallbackContext context)
    {
        foreach (var player in GameStateManager.Instance.Players)
        {
            player.actions[_nextAction.action.id.ToString()].started -= ChangeSceneDelayed;
        }
        if(this)
        {
            Invoke(nameof(ChangeScene), 0);
        }
    }
}
