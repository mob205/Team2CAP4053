using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class BlackoutRoom : MonoBehaviour
{
    [Tooltip("References to walls in the level that should be made semi-transparent when a player enters it1")]
    [SerializeField] private GameObject[] frontWalls;

    [Tooltip("The transparency of front walls when the room is active (i.e. a player is in it)")]
    [Range(0f, 1f)]
    [SerializeField] private float _activeWallTransparency;

    [Tooltip("The transparency of the dark cube that obscures the room when the room is inactive")]
    [Range(0f, 1f)]
    [SerializeField] private float _inactiveRoomTransparency;

    [Tooltip("The amount of time, in seconds, that it should take for the transparencies to fully change")]
    [SerializeField] private float _transitionTime; 


    private Material _mat;
    private Material[] _wallMaterials;

    private int _playerCount = 0;

    private bool _isActive;

    private void Start()
    {
        _mat = GetComponent<Renderer>().material;

        _wallMaterials = new Material[frontWalls.Length];
        for(int i = 0; i < frontWalls.Length; ++i)
        {
            _wallMaterials[i] = frontWalls[i].GetComponent<Renderer>().material;
        }
    }

    private void Update()
    {
        float deltaWall;
        float deltaRoom;
        if (_transitionTime != 0)
        {
            deltaWall = Time.deltaTime * ((1 - _activeWallTransparency) / _transitionTime);
            deltaRoom = Time.deltaTime * _inactiveRoomTransparency / _transitionTime;

            Debug.Log($"dWall: {deltaWall} | dRoom: {deltaRoom}");
        }
        else
        {
            deltaWall = 1;
            deltaRoom = 1;
        }

        int direction;
        if(_isActive)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        UpdateColors(deltaWall * direction, deltaRoom * direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        ++_playerCount; 
        if(_playerCount > 0)
        {
            _isActive = true;
            //Debug.Log("Entering");
            //SetColors(wallTransparency, 0);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        --_playerCount;
        if(_playerCount <= 0)
        {
            _isActive = false;
            //Debug.Log("Exiting");
            //SetColors(1, darknessTransparency);
        }
    }

    private void UpdateColors(float deltaWall, float deltaRoom)
    {

        Color curColor = _mat.color;
        _mat.color = new Color(curColor.r, curColor.g, curColor.b, Mathf.Clamp(curColor.a += deltaRoom, 0, _inactiveRoomTransparency));


        // Set walls to semi-transparent
        foreach (var wallMat in _wallMaterials)
        {
            curColor = wallMat.color;
            wallMat.color = new Color(curColor.r, curColor.g, curColor.b, Mathf.Clamp(curColor.a += deltaWall, _activeWallTransparency, 1));
        }
    }
}
