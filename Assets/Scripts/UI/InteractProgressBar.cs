using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractProgressBar : MonoBehaviour
{
    [SerializeField] private DurationInteractable _interactable;
    private Slider _progressBarUI;

    private void Start()
    {
        _progressBarUI = GetComponentInChildren<Slider>();

        if (!_interactable)
        {
            Debug.LogError($"No interactable provided to progress bar {name}. Destroying");
            Destroy(this);
        }
        if (!_progressBarUI)
        {
            Debug.LogError($"No slider UI found on progress bar {name}. Destroying");
            Destroy(this);
        }
    }

    private void Update()
    {
        // This can be hooked onto events on EnemySpawner if needed
        if(_interactable.IsInProgress && !_progressBarUI.gameObject.activeSelf)
        {
            _progressBarUI.gameObject.SetActive(true);
            _progressBarUI.value = 1 - (_interactable.TimeRemaining / _interactable.MaxDuration);
        }
        else if(!_interactable.IsInProgress && _progressBarUI.gameObject.activeSelf)
        {
            _progressBarUI.gameObject.SetActive(false);
        }
    }
}
