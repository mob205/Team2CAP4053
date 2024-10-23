using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormIndicator : MonoBehaviour
{

    [SerializeField] RectTransform _alertObj;

    private Camera _cam;
    private Canvas _canvas;

    private void Start()
    {
        _cam = Camera.main;
        _canvas = GetComponent<Canvas>();
        WormAI.OnWormAttack += AlertWormAttack;
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
