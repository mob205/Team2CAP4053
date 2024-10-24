using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAttackCameraShake : MonoBehaviour
{
    [Tooltip("The amount of amplitude to add to the camera shake when a worm enters the level")]
    [SerializeField] private float _ampGainPerWorm;

    private CinemachineBasicMultiChannelPerlin _noise;
    private WormIndicator _indicator;

    private void Awake()
    {
        _noise = GetComponentInChildren<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Start()
    {
        _indicator = FindObjectOfType<WormIndicator>();
        if(_indicator)
        {
            _indicator.OnWormEnter.AddListener(AddShake);
            _indicator.OnWormExit.AddListener(RemoveShake);
        }
        else
        {
            Destroy(this);
        }
        GameStateManager.Instance.OnGameEnd.AddListener(ClearShake);
    }

    private void AddShake(WormAI worm)
    {
        _noise.m_AmplitudeGain += _ampGainPerWorm;
    }

    private void RemoveShake(WormAI worm)
    {
        _noise.m_AmplitudeGain -= _ampGainPerWorm;
    }
    private void ClearShake()
    {
        _noise.m_AmplitudeGain = 0.0f;
    }
}
