using UnityEngine;
using UnityEngine.Audio;

public class VolumeLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;

    private void Start()
    {
        _mixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
    }
}
