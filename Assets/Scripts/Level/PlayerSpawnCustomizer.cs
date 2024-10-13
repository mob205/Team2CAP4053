using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnCustomizer : MonoBehaviour
{
    [field: SerializeField] public int MaxPlayers { get; private set; } = 4;

    [SerializeField] private Transform[] m_spawnPoints;
    [SerializeField] private Material[] m_materials;

    // Array to hold different player prefabs
    [SerializeField] private GameObject[] m_playerPrefabs;

    private int m_numPlayers = 0;

    public void OnPlayerJoined(PlayerInput input)
    {
        // Select the appropriate player prefab for the current player
        GameObject playerPrefab = m_playerPrefabs[m_numPlayers % m_playerPrefabs.Length];

        // Instantiate the player at the spawn point
        GameObject player = Instantiate(playerPrefab, m_spawnPoints[m_numPlayers % m_spawnPoints.Length].position, Quaternion.identity);

        // Assign the input to the new player
        player.GetComponent<PlayerInput>().SwitchCurrentControlScheme(input.currentControlScheme);

        // Customize the player with a material
        player.GetComponent<MeshRenderer>().SetMaterials(new List<Material> { m_materials[m_numPlayers % m_materials.Length] });

        // Update player count
        ++m_numPlayers;
    }
}
