using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private ToolType _hammerType; 

    public List<PlayerInteractor> NearbyPlayers { get; private set; } = new List<PlayerInteractor>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerInteractor interactor))
        {
            NearbyPlayers.Add(interactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out PlayerInteractor interactor) && NearbyPlayers.Contains(interactor))
        {
            NearbyPlayers.Remove(interactor);
        }
    }
}
