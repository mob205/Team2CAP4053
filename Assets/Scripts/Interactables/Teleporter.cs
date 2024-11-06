using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform TeleportDestination;
    public LayerMask TeleportLayers;
 
    private void OnTriggerEnter(Collider collision)
    {
        var closestPlayer = collision.gameObject;

        if (Helpers.IsInMask(TeleportLayers, closestPlayer.layer))
        {
            closestPlayer.transform.position = TeleportDestination.position;
        }
    }
}
