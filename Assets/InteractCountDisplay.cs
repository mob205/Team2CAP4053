using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractCountDisplay : MonoBehaviour
{
    [SerializeField] private DurationInteractable _interactable;
    private TextMeshPro _text;

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshPro>();

        if (!_interactable)
        {
            Debug.LogError($"No interactable provided to interactor count {name}. Destroying");
            Destroy(this);
        }
        if (!_text)
        {
            Debug.LogError($"No text found on interactor count {name}. Destroying");
            Destroy(this);
        }
    }

    private void Update()
    {
        // This can be hooked onto events on EnemySpawner if needed
        if (_interactable.CurrentNumInteractors > 0 && !_text.gameObject.activeSelf)
        {
            _text.gameObject.SetActive(true);
            _text.text = $"{_interactable.CurrentNumInteractors} / {_interactable.NumInteractorsRequired}";
        }
        else if(_interactable.CurrentNumInteractors == 0 && _text.gameObject.activeSelf)
        {
            _text.gameObject.SetActive(false);
        }
    }
}
