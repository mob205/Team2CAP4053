using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _pregameMusic;
    [SerializeField] private AudioSource _transition;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _defeatMusic;

    private void Start()
    {
        _pregameMusic.Play();

        ReadyUpManager.Instance.OnReadyUp.AddListener(PlayTransition);
        GameStateManager.Instance.OnGameLose.AddListener(PlayDefeat);
    }
    private void PlayTransition()
    {
        _pregameMusic.Stop();
        if(_transition.clip != null)
        {
            _transition.Play();
            _gameMusic.PlayDelayed(_transition.clip.length * .8f);
        }
        else
        {
            _gameMusic.Play();
        }
    }
    private void PlayDefeat()
    {
        _pregameMusic.Stop();
        _transition.Stop();
        _gameMusic.Stop();
        _defeatMusic.Play();
    }
}
