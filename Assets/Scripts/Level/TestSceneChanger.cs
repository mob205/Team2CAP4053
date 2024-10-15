using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(2);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(2);
    }
}
