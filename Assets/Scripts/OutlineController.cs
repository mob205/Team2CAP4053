using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private float _flickerOnDuration = .5f;
    [SerializeField] private float _flickerOffDuration = .5f;

    private Outline _outline;

    private Coroutine _flickerCoroutine;

    private Color _enabledColor;
    private Color _disabledColor;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        if(!_outline)
        {
            Destroy(this);
        }
        _enabledColor = _outline.OutlineColor;
        _disabledColor = Color.clear;
    }

    private void Start()
    {
        _outline.OutlineColor = _disabledColor;
    }

    public void SetInactive()
    {
        StopFlickering();
        _outline.OutlineColor = _disabledColor;
    }
    public void SetActive()
    {
        StopFlickering();
        _outline.OutlineColor = _enabledColor;
    }

    public void StartFlickering()
    {
        StopFlickering();
        _flickerCoroutine = StartCoroutine(FlickerOutline());
    }

    public void StopFlickering()
    {
        if(_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
        }
    }

    private IEnumerator FlickerOutline()
    {
        while(true)
        {
            _outline.OutlineColor = _enabledColor;
            yield return new WaitForSeconds(_flickerOnDuration);
            _outline.OutlineColor = _disabledColor;
            yield return new WaitForSeconds(_flickerOffDuration);
        }
    }
}
