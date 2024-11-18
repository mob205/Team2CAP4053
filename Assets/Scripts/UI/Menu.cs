using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AsyncOperation _changeToLoadingScreen;
    private void Start()
    {
        _changeToLoadingScreen = SceneManager.LoadSceneAsync("LoadingScreen");
        _changeToLoadingScreen.allowSceneActivation = false;
    }
    public void OnPlayButton() {
		SavedData.NextScene = 1;
        _changeToLoadingScreen.allowSceneActivation = true;
	}
	
    public void OnQuitButton() {
	Application.Quit();
	}

}
