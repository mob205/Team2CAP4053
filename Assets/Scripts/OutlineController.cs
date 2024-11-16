using cakeslice;
using System.Collections;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private float _flickerOnDuration = .5f;
    [SerializeField] private float _flickerOffDuration = .5f;

    private Outline _outline;

    private Coroutine _flickerCoroutine;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void Start()
    {
        _outline.enabled = false;
    }

    public void SetInactive()
    {
        StopFlickering();
        _outline.enabled = false;
    }
    public void SetActive()
    {
        StopFlickering();
        _outline.enabled = true;
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
            _outline.enabled = true;
            yield return new WaitForSeconds(_flickerOnDuration);
            _outline.enabled = false;
            yield return new WaitForSeconds(_flickerOffDuration);
        }
    }
}
