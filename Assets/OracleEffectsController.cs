using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OracleEffectsController : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 1.0f;
    [SerializeField] private float _fadeOutDuration = 1.0f;

    private ParticleSystem _particles;
    private void Start()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, _fadeInDuration).SetEase(Ease.OutSine);
    }

    public void OnKill()
    {
        transform.DOKill();
        transform.SetParent(null, true);
        _particles.Stop();
        transform.DOScale(Vector3.zero, _fadeOutDuration).SetEase(Ease.InElastic).OnComplete(() => Destroy(gameObject));
    }
}
