using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class FXController : MonoBehaviour
{
    [SerializeField] private AnimationCurve _intensityCurve;
    [SerializeField] private float _resetSpeed;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private float _minPitch;

    private PostProcessVolume _volume;
    private ChromaticAberration _aberration;
    private Grain _grain;

    WindupEnemySpawner[] _spawners;

    private float _currentIntensity; 

    private void Awake()
    {
        _volume = GetComponent<PostProcessVolume>();
        _volume.sharedProfile.TryGetSettings(out _aberration);
        _volume.sharedProfile.TryGetSettings(out _grain);
    }

    private void Start()
    {
        _spawners = FindObjectsByType<WindupEnemySpawner>(FindObjectsSortMode.None);
        _volume.enabled = true;

        GameStateManager.Instance.OnGameEnd.AddListener(StopAudioEffects);
    }

    void Update()
    {
        float ratio = 0;
        foreach(var spawner in _spawners)
        {
            ratio = Mathf.Max(spawner.WindupRatio, ratio);
        }
        UpdateIntensity(_intensityCurve.Evaluate(ratio));
    }
    private void UpdateIntensity(float newIntensity)
    {
        _currentIntensity = Mathf.MoveTowards(_currentIntensity, newIntensity, _resetSpeed * Time.deltaTime);
        _aberration.intensity.value = _currentIntensity;
        _grain.intensity.value = _currentIntensity;
        _mixer.SetFloat("Pitch", 1 - _currentIntensity * (1 - _minPitch));
    }

    private void StopAudioEffects()
    {
        _mixer.SetFloat("Pitch", 1);
    }
}
