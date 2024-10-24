using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormIndicatorUI : MonoBehaviour
{
    [SerializeField] RectTransform _alertObj;

    private Camera _cam;
    private Canvas _canvas;
    private WormIndicator _indicator;

    private void Start()
    {

        _cam = Camera.main;
        _canvas = GetComponent<Canvas>();

        _indicator = FindObjectOfType<WormIndicator>();
        if(_indicator)
        {
            _indicator.OnWormEnter.AddListener((worm) => AlertWormAttack(worm.transform.position));
        }
    }

    public void AlertWormAttack(Vector3 location)
    {
        if(_cam && _alertObj)
        {
            var pos = _cam.WorldToScreenPoint(location);

            pos.x = Mathf.Clamp(pos.x, 0 - _alertObj.rect.x, _canvas.renderingDisplaySize.x + _alertObj.rect.x);
            pos.y = Mathf.Clamp(pos.y, 0 - _alertObj.rect.y, _canvas.renderingDisplaySize.y + _alertObj.rect.y);

            Instantiate(_alertObj, pos, Quaternion.identity, transform);
        }
    }
}
