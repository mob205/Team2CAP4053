using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingAlertUI : MonoBehaviour
{
    [SerializeField] private float _durationOn;
    [SerializeField] private float _durationOff;
    [SerializeField] private float _fadeRate;
    [SerializeField] private int _numFlicker;

    private Image _alertImage;

    private void Start()
    {
        _alertImage = GetComponent<Image>();
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        var curColor = _alertImage.color;
        curColor.a = 0;
        _alertImage.color = curColor;

        for(int i = 0; i < _numFlicker; ++i)
        {
            while (curColor.a < 1)
            {
                curColor.a += _fadeRate * Time.deltaTime;
                _alertImage.color = curColor;
                yield return 0;
            }

            yield return new WaitForSeconds(_durationOn);

            curColor.a = 1;
            _alertImage.color = curColor;

            while(curColor.a > 0)
            {
                curColor.a -= _fadeRate * Time.deltaTime;
                _alertImage.color = curColor;
                yield return 0;
            }
            curColor.a = 0;
            yield return new WaitForSeconds(_durationOff);
        }
        Destroy(gameObject);
    }

}
