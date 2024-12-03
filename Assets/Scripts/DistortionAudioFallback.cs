using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DistortionAudioFallback : MonoBehaviour
{
    [SerializeField] AudioMixer _mixer;
    private void Start()
    {
        _mixer.SetFloat("Pitch", 1);
    }
}
