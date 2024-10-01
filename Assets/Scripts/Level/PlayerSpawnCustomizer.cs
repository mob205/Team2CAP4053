using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnCustomizer : MonoBehaviour
{
    [field: SerializeField] public int MaxPlayers { get; private set; } = 4;

    [SerializeField] private Transform[] m_spawnPoints;
    [SerializeField] private Material[] m_materials;

    private int m_numPlayers = 0;

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        GameObject player = input.gameObject;

        player.transform.position = m_spawnPoints[m_numPlayers % m_spawnPoints.Length].position;
        player.GetComponent<MeshRenderer>().SetMaterials(new List<Material>{ m_materials[m_numPlayers % m_materials.Length]});

        ++m_numPlayers;
    }
}
