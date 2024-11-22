using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeBar : MonoBehaviour
{
    [SerializeField] private GameObject[] _barPieces;
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private AnimationCurve _volumeCurve;


    private int _currentVolume;
    private float _volumePerStep;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetInt("VolumeBarTick", _barPieces.Length));
    }

    public void DecreaseVolume()
    {
        SetVolume(_currentVolume - 1);
    }
    public void IncreaseVolume()
    {
        SetVolume(_currentVolume + 1);
    }
    public void SetVolume(int volumeStep)
    {
        _currentVolume = Mathf.Clamp(volumeStep, 0, _barPieces.Length);

        float volume = _volumeCurve.Evaluate(_currentVolume);
        _mixer.SetFloat("Volume", _volumeCurve.Evaluate(_currentVolume));
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("VolumeBarTick", volumeStep);

        for(int i = 0; i < _barPieces.Length; i++)
        {
            _barPieces[i].SetActive(i < _currentVolume);
        }
    }
}
