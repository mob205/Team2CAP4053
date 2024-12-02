using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

[Serializable]
class LevelCutscene
{
    public string Level;
    public VideoClip Video;
    public int SongIndex;
}

[Serializable]
struct CutsceneMusic
{
    public AudioClip Intro;
    public AudioClip Loop;
}

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private LevelCutscene[] _cutscenes;
    [SerializeField] private GameObject _skipButton;
    [SerializeField] private GameObject _throbber;
    [SerializeField] private GameObject _videoUI;

    [SerializeField] private CutsceneMusic[] _musicTracks;

    [SerializeField] private AudioSource _songIntro;
    [SerializeField] private AudioSource _songLoop;

    private VideoPlayer _player;
    private bool _isPlayingCutscene;
    private bool _canSkip;


    private void Awake()
    {
        _player = GetComponent<VideoPlayer>();
    }
    private void Start()
    {

        // The game launches to the loading screen to play the intro cutscene
        // So, the menu needs to be loaded
        LevelManager.StartLoadingLevel(LevelManager.LoadingLevel);

        _skipButton.SetActive(false);

        if (!LevelManager.ShouldPlayCutscene)
        {
            LevelManager.FinishLoading();
            return;
        }


        var cutscene = GetCutscene(LevelManager.LoadingLevel);
        if(cutscene != null)
        {
            PlayCutscene(cutscene);
        }
        else
        {
            LevelManager.FinishLoading();
        }
    }
    private void Update()
    {
        if(_isPlayingCutscene && LevelManager.GetLoadingProgress() >= .899)
        {
            _skipButton.SetActive(true);
            _canSkip = true;
        }
    }
    private LevelCutscene GetCutscene(string level)
    {
        foreach(var cutscene in _cutscenes)
        {
            if(cutscene.Level == level)
            {
                return cutscene;
            }
        }
        return null;
    }
    public void OnSkip(InputAction.CallbackContext context)
    {
        if(context.started && _canSkip)
        {
            EndCutscene();
        }
    }

    private void PlayCutscene(LevelCutscene cutscene)
    {
        _videoUI.SetActive(true);

        _throbber.SetActive(false);
        _player.clip = cutscene.Video;
        _player.Play();

        CutsceneMusic music = _musicTracks[cutscene.SongIndex];
        _songIntro.clip = music.Intro;
        _songIntro.Play();
        _songLoop.clip = music.Loop;
        _songLoop.PlayDelayed(_songIntro.clip.length);

        Invoke(nameof(EndCutscene), (float) cutscene.Video.length);
        _isPlayingCutscene = true;
    }
    private void EndCutscene()
    {
        LevelManager.FinishLoading();
        _isPlayingCutscene = false;
    }
}
