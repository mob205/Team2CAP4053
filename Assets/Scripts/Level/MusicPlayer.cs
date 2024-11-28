using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _pregameMusic;
    [SerializeField] private AudioSource _transition;
    [SerializeField] private AudioSource _gameMusic;

    private void Start()
    {
        _pregameMusic.Play();

        ReadyUpManager.Instance.OnReadyUp.AddListener(PlayTransition);
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
}
