using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnCustomizer : MonoBehaviour
{
    [field: SerializeField] public int MaxPlayers { get; private set; } = 4;

    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject[] _playerPrefabs;

    private PlayerInputManager _inputManager;

    private int _numPlayers = 0;


    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
        _inputManager.playerPrefab = _playerPrefabs[0];
    }
    public void OnPlayerJoined(PlayerInput input)
    {
        GameObject player = input.gameObject;

        player.transform.position = _spawnPoints[_numPlayers % _spawnPoints.Length].position;

        ++_numPlayers;
        _inputManager.playerPrefab = _playerPrefabs[_numPlayers % _playerPrefabs.Length];
    }
}
