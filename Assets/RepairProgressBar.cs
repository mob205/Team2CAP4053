using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairProgressBar : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;
    private Slider _progressBarUI;

    private void Start()
    {
        _spawner = GetComponentInParent<EnemySpawner>();
        _progressBarUI = GetComponentInChildren<Slider>();

        if (!_spawner)
        {
            Debug.LogError("No enemy spawner provided to this RepairProgressBar. Destroying.");
            Destroy(this);
        }
        if (!_progressBarUI)
        {
            Debug.LogError("No slider UI is child to this RepairProgressBar. Destroying.");
            Destroy(this);
        }
    }

    private void Update()
    {
        // This can be hooked onto events on EnemySpawner if needed
        if(_spawner.IsRepairing)
        {
            _progressBarUI.gameObject.SetActive(true);
            _progressBarUI.value = 1 - (_spawner.CurrentRepairTimer / _spawner.MaxRepairDuration);
        }
        else
        {
            _progressBarUI.gameObject.SetActive(false);
        }
    }
}
