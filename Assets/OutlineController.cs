using cakeslice;
using System.Collections;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private int _activeColor;
    [SerializeField] private int _inactiveColor = 2;

    [SerializeField] private float _flickerOnDuration = .5f;
    [SerializeField] private float _flickerOffDuration = .5f;

    private Outline _outline;

    private Coroutine _flickerCoroutine;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void SetInactive()
    {
        if(_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
        }
        SetColor(_inactiveColor);
    }
    public void SetActive()
    {
        if(_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
        }
        SetColor(_activeColor);
    }

    public void StartFlickering()
    {
        if(_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
        }
        _flickerCoroutine = StartCoroutine(FlickerOutline());
    }

    private void SetColor(int color)
    {
        _outline.color = color;
    }

    private IEnumerator FlickerOutline()
    {
        while(true)
        {
            Debug.Log("ON");
            SetColor(_activeColor);
            yield return new WaitForSeconds(_flickerOnDuration);
            Debug.Log("OFF");
            SetColor(_inactiveColor);
            yield return new WaitForSeconds(_flickerOffDuration);
        }
    }
}
