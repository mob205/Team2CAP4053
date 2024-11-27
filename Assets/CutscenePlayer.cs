using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

[Serializable]
struct LevelCutscene
{
    public string Level;
    public VideoClip Cutscene;
}
public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private LevelCutscene[] _cutscenes;
    [SerializeField] private GameObject _skipButton;
    [SerializeField] private GameObject _throbber;

    private VideoPlayer _player;
    private bool _isPlayingCutscene;
    private bool _canSkip;

    private void Awake()
    {
        _player = GetComponent<VideoPlayer>();
    }
    private void Start()
    {
        LevelManager.LoadLevel(LevelManager.LoadingLevel);

        _skipButton.SetActive(false);

        var video = GetVideo(LevelManager.LoadingLevel);
        if(video != null)
        {
            PlayCutscene(video);
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
    private VideoClip GetVideo(string level)
    {
        foreach(var cutscene in _cutscenes)
        {
            if(cutscene.Level == level)
            {
                return cutscene.Cutscene;
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

    private void PlayCutscene(VideoClip video)
    {
        _throbber.SetActive(false);
        _player.clip = video;
        _player.Play();
        Invoke(nameof(EndCutscene), (float) video.length);
        _isPlayingCutscene = true;
    }
    private void EndCutscene()
    {
        LevelManager.FinishLoading();
        _isPlayingCutscene = false;
    }
}
