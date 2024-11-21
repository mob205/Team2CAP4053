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
        _currentVolume = _barPieces.Length;
    }

    public void DecreaseVolume()
    {
        SetVolume(_currentVolume - 1);
    }
    public void IncreaseVolume()
    {
        SetVolume(_currentVolume + 1);
    }
    public void SetVolume(int volume)
    {
        _currentVolume = Mathf.Clamp(volume, 0, _barPieces.Length);
        _mixer.SetFloat("Volume", _volumeCurve.Evaluate(_currentVolume));

        for(int i = 0; i < _barPieces.Length; i++)
        {
            _barPieces[i].SetActive(i < _currentVolume);
        }
    }
}
