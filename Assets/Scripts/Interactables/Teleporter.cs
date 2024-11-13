using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform TeleportDestination;
    [SerializeField] private LayerMask TeleportLayers;
    [SerializeField] private ParticleSystem _teleportParticles;
    [SerializeField] private AudioEvent _teleportSFX;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        var closestPlayer = collision.gameObject;

        if (Helpers.IsInMask(TeleportLayers, closestPlayer.layer))
        {
            if(_teleportParticles)
            {
                Instantiate(_teleportParticles, closestPlayer.transform.position, Quaternion.identity);
                Instantiate(_teleportParticles, TeleportDestination.position, Quaternion.identity);
            }
            if(_teleportSFX)
            {
                _teleportSFX.Play(_audioSource);
            }

            closestPlayer.transform.position = TeleportDestination.position;
        }
    }
}
