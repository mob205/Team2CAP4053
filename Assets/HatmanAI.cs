using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatmanAI : Enemy
{
    [Tooltip("Delay after teleporting to a player before killing them")]
    [SerializeField] private float _killDelay;

    [Tooltip("Offset away from player to teleport to")]
    [SerializeField] private float _teleportDistance;

    [Tooltip("Distance the cat must be from the target player to prevent hatman from attacking")]
    [SerializeField] private float _catDetectionRange;

    [Tooltip("Layers that block the hatman from teleporting")]
    [SerializeField] private LayerMask _blockingLayer;

    [Tooltip("Layers that will cause the hatman to spare target player, if nearby")]
    [SerializeField] private LayerMask _spareLayers;
       
    private PlayerHealth[] _players;

    private static Vector3[] _teleportDirections = { Vector3.forward, -Vector3.forward, Vector3.left, -Vector3.left };
    private void Start()
    {
        _players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        foreach(var player in _players)
        {
            if (player == null || player.IsDead) continue;

            transform.position = GetTeleportLocation(player.transform.position);
            yield return new WaitForSeconds(_killDelay);

            // Kill the player if a spare object is not nearby
            if(!Physics.CheckSphere(player.transform.position, _catDetectionRange, _spareLayers))
            {
                player.Kill();
            }
        }
        Kill();
    }
    private Vector3 GetTeleportLocation(Vector3 player)
    {
        foreach(var direction in _teleportDirections)
        {
            // Only teleport in this direction if that spot isn't being blocked
            if(!Physics.Raycast(player, direction, _teleportDistance, _blockingLayer))
            {
                return (direction * _teleportDistance) + player;
            }
        }
        // Fallback, might end up in a wall still
        return (_teleportDirections[0] * _teleportDistance) + player;
    }
}
