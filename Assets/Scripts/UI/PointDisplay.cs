using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PointDisplay : MonoBehaviour
{
    [SerializeField] private float _updateDelay;

    private PointCalculator _points;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _points = GameStateManager.Instance.GetComponent<PointCalculator>();
        if(!_points)
        {
            Debug.Log("No PointsCalculator found. Deleting points display UI.");
            Destroy(this);
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _text.text = "Points: " + Mathf.CeilToInt(_points.Points).ToString();
        Invoke(nameof(UpdateDisplay), _updateDelay);
    }
}
