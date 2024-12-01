using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

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
        var target = collision.gameObject;

        if (Helpers.IsInMask(TeleportLayers, target.layer))
        {
            if(_teleportParticles)
            {
                Instantiate(_teleportParticles, target.transform.position, Quaternion.identity);
                Instantiate(_teleportParticles, TeleportDestination.position, Quaternion.identity);
            }
            if(_teleportSFX)
            {
                _teleportSFX.Play(_audioSource);
            }

            if(target.transform.parent && target.transform.parent.TryGetComponent(out NavMeshAgent agent))
            {
                agent.Warp(TeleportDestination.position);
                if(agent.TryGetComponent(out ITeleportable teleportable))
                {
                    teleportable.OnTeleport();
                }
            }
            else
            {
                target.transform.position = TeleportDestination.position;
            }
        }
    }
}
