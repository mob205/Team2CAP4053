using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string _loadingScene;
    [SerializeField] private string _levelScene;

    private AsyncOperation _changeToLoadingScreen;
    private void Start()
    {
        _changeToLoadingScreen = SceneManager.LoadSceneAsync(_loadingScene);
        _changeToLoadingScreen.allowSceneActivation = false;
    }
    public void OnPlayButton() 
    {
		SavedData.NextScene = _levelScene;
        _changeToLoadingScreen.allowSceneActivation = true;
	}
	
    public void OnQuitButton()
    {
	    Application.Quit();
	}

}
