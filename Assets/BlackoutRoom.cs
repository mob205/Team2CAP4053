using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutRoom : MonoBehaviour
{
    [SerializeField] private GameObject[] frontWalls;

    [SerializeField] private float wallTransparency;
    [SerializeField] private float darknessTransparency;

    private Material _mat;
    private int _count = 0;

    private void Start()
    {
        _mat = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        ++_count; 
        if(_count > 0)
        {
            Debug.Log("Entering");
            SetColors(wallTransparency, 0);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        --_count;
        if(_count <= 0)
        {
            Debug.Log("Exiting");
            SetColors(1, darknessTransparency);
        }
    }

    private void SetColors(float wallTransparency, float darknessTransparency)
    {

        Color curColor = _mat.color;
        _mat.color = new Color(curColor.r, curColor.g, curColor.b, darknessTransparency);


        // Set walls to semi-transparent
        foreach (var wall in frontWalls)
        {
            Material wallMat = wall.GetComponent<Renderer>().material;
            curColor = wallMat.color;
            wallMat.color = new Color(curColor.r, curColor.g, curColor.b, wallTransparency);
        }
    }
}
